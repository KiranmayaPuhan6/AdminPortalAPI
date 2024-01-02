using BCrypt.Net;
using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTY2NDE5OTEyOCwiaWF0IjoxNjY0MTk5MTI4fQ.FZGfQZKyicxnX5NkwWXzPushKnbT9i_3NNThj-sYpUQ";
        private const int JWT_TOKEN_VALIDITY_MINS = 60;
        private List<UserAccount> users;
        private UserAccount adminUser;
        // public JwtTokenHandler()
        //{
        //users = new List<UserAccount>
        //{
        //    new UserAccount{ UserName = "admin", Password = "admin@123", Role = "Administrator" },
        //    new UserAccount{ UserName = "user01", Password = "user01@123", Role = "HR" }
        //};
        //}
        public JwtTokenHandler()
        {
          /*  adminUser = new UserAccount()
            {
                UserName = "admin1",
                Password = "password1",
                Role = "Admin"
            };*/
        }
        private void GetUsers()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://localhost:10012/api/user/Validate");
            response.Wait();
            if (response.IsCompleted)
            {
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync();
                    data.Wait();

                    this.users = JsonConvert.DeserializeObject(data.Result, typeof(List<UserAccount>)) as List<UserAccount>;
                }
              //  this.users.Add(adminUser);
            }
        }

        public AuthenticationResponse GenerateToken(AuthenticationRequest authenticationRequest)
        {
            GetUsers();
            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
            {
                return null;
            }
            var user = users.Where(x => x.UserName == authenticationRequest.UserName && BCrypt.Net.BCrypt.Verify(authenticationRequest.Password,x.Password)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            var tokentExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            });
            var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                );
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokentExpiryTimeStamp,                
                SigningCredentials = signingCredentials
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                 UserName = user.UserName,
                 ExpiresIn = (int)tokentExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                 JwtToken = token,
                 Role=user.Role,
                 UserId=user.Id,
            };
        }
    }
}
