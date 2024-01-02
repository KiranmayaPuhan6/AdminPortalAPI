using AutoMapper;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Security.Cryptography;
using UserAPI.Models.Domain;
using UserAPI.Models.Dto;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository userRepository;
        public readonly IMapper mapper;
        private readonly JwtTokenHandler jwtAuthenticationManager;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _config;

        public UserController(IUserRepository _userRepository, IMapper _mapper, JwtTokenHandler _jwtAuthenticationManager, IWebHostEnvironment _environment, IConfiguration config)
        {
            userRepository = _userRepository;
            mapper = _mapper;
            jwtAuthenticationManager = _jwtAuthenticationManager;
            environment = _environment;
            _config = config;
        }

        [HttpPost("Authenticate")]
        public ActionResult<AuthenticationResponse> LoginUser([FromBody] AuthenticationRequest _user)
        {

            var authenticationResponse = this.jwtAuthenticationManager.GenerateToken(_user);
            if (authenticationResponse == null)
            {
                return Unauthorized();
            }
            return authenticationResponse;

            //var token = jwtAuthenticationManager.Authenticate(_user.UserName, _user.Password);
            //if (token == null)
            //{
            //    return Unauthorized();
            //}
            //return Ok(token);
        }
        



        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var userInfo = await userRepository.GetUserInfoAsync(id);
            if (userInfo != null)
            {
                DeleteImage(userInfo.ActualFileUrl);
                var user = await userRepository.DeleteUserAsync(id);
                var userDto = mapper.Map<UserDto>(user);
                return Ok(true);
            }
            else
            {
               return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync([FromForm]User user)
        {
            Console.WriteLine("sdgfhjklhgfhjg");
            var users = userRepository.GetUsers();
            if (users.Any(x => x.UserName == user.UserName))
            {
                return BadRequest("This username already exists");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //else if (user.Contact.ToString().Length != 10)
            //{
            //    return BadRequest("The contact number is invalid");
            //}
            else
            {
                string path;
                if (user.FileUri != null)
                {
                     path = await UploadImage(user.FileUri);
                    if(path == "Not a valid type")
                    {
                        return BadRequest("Not a valid type");
                    }
                }
                else
                {
                     path = null;
                }    
                    var userModel = new User()
                    {

                        UserName = user.UserName,
                        Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                        AppRole = user.AppRole,
                        Role = user.Role,
                        AppId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Contact = user.Contact,
                        CreatedBy = user.UserName,
                        CreatedTime = DateTime.Now,
                        ActualFileUrl = path
                    };
                //var userModel = mapper.Map<User>(user); 
                var userMain = await userRepository.RegisterUserAsync(userModel);
                //var userDto = mapper.Map<UserDto>(user);
                return Ok(user);
            }

        }

    [HttpGet("id")]
   [Authorize(Roles = "Admin,User,Owner")]
    public async Task<IActionResult> GetUserInfoAsync(int id)
    {
        var user = await userRepository.GetUserInfoAsync(id);
        if (user == null)
        {
            return NoContent();
        }
        var userDto = mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

        [HttpGet("image")]
        [Authorize(Roles = "Admin,User,Owner")]
        public async Task<IActionResult> GetUserImageAsync(string _userName)
        {
            var users = userRepository.GetUsers();
            foreach(var user in users)
            {
                if(user.UserName == _userName)
                {
                    return Ok(user.ActualFileUrl);
                }
            }
            
            return null;
        }
        [HttpGet("UserName")]
    public async Task<IActionResult> GetUserByNameAsync(string name)
    {
        var user = await userRepository.GetUserByNameAsync(name);
        if (user == null)
        {
            return NoContent();
        }
        var userNameDto = mapper.Map<UserDto>(user);
        return Ok(userNameDto);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<List<User>> GetAllUsers()
    {
        var users = userRepository.GetUsers();
        return Ok(users);
    }


    [HttpGet("Validate")]
    public ActionResult<List<User>> GetAllUsersForValidation()
    {
        var users = userRepository.GetUsers();
        return Ok(users);
    }


        [HttpPut]
       [Authorize(Roles = "Admin,User,Owner")]
        public async Task<IActionResult> UpdateUserAsync(int id, User _user, string useName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var user = await userRepository.UpdateUserAsync(id, _user, useName);
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
        }


    [HttpGet("Role")]

        public async Task<IActionResult> GetRoleById(int id)
        {
            var user = await userRepository.GetRoleById(id);
            if (user == null)
            {
                return NoContent();
            }
            var userDto = mapper.Map<UserDto>(user);
            return Ok(userDto.AppRole);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var token = HelperMethods.RandomNumberGenerator.Generate(100000,999999);
            var tokenExpiry = DateTime.Now.AddMinutes(30);
            var value = await userRepository.ResetPasswordTokenAsync(email,token,tokenExpiry);
            if(value == null)
            {
                return BadRequest();
            }
            else
            {
                var body = "Your one time OTP is " + token + ".This OTP will expire in 30 minutes.";
                SendEmail(email, body);
                return Ok(value);

            }
            
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {
            var value = await userRepository.ChangePasswordAsync(request);
            if(value == null) 
            {
                return BadRequest();
            }
            return Ok(value);
        }
        public async Task<string> UploadImage(IFormFile objfile)
        {
            try
            {
                string guid = Guid.NewGuid().ToString();
                string[] allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
                if (objfile.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Upload\\");
                    }
                    string extension = Path.GetExtension(objfile.FileName);
                    if (extension.Equals(allowedExtension[0]) || extension.Equals(allowedExtension[1]) || extension.Equals(allowedExtension[2]))
                    {
                        using (FileStream fileStream = System.IO.File.Create(environment.WebRootPath + "\\Upload\\" + guid + objfile.FileName))
                        {
                            objfile.CopyTo(fileStream);
                            fileStream.Flush();
                            return "\\Upload\\" + guid + objfile.FileName;
                        }
                    }
                    else
                    {
                        return "Not a valid type";
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string SendEmail(string toMail, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(toMail));
            email.Subject = "Reset Password";
            email.Body = new TextPart(TextFormat.Html) { Text = body }; 
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
            return "Mail sent successfully";
        }
        public async Task<string> DeleteImage(string path)
        {
            FileInfo fileInfo = new FileInfo(environment.WebRootPath + path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                return "Deleted";
            }
            else
            {
                return null;
            }
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}


