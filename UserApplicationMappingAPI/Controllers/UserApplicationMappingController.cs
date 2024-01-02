using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserApplicationMappingAPI.Model.Domain;
using UserApplicationMappingAPI.Models.DTO;
using UserApplicationMappingAPI.Repositories.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApplicationMappingController : ControllerBase
    {
        public readonly IUserApplicationMappingRepository userAppMapRepository;
        public readonly IMapper mapper;


        public UserApplicationMappingController(IUserApplicationMappingRepository _userApplicationRepository, IMapper _mapper)
        {
            userAppMapRepository = _userApplicationRepository;
            mapper = _mapper;
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserApplicationAsync(int id)
        {
            var userApp = await userAppMapRepository.DeleteUserApplicationAsync(id);
            if (userApp == null)
            {
                return BadRequest(false);
            }
            else
            {
                var userAppDto = mapper.Map<UserApplicationMappingDto>(userApp);
                return Ok(userAppDto);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserAppAsync(string userName,UserApplicationMapping userApp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var userAppModel = new UserApplicationMapping()
                {

                    AppId = userApp.AppId,
                    UserId = userApp.UserId,
                    CreatedBy= userName,
                    CreatedTime = DateTime.Now,

                };

                userApp = await userAppMapRepository.AddUserApplicationAsync(userName,userAppModel);
                var userAppDto = mapper.Map<UserApplicationMappingDto>(userApp);
                return Ok(userAppDto);

            }
        }

        [HttpGet("id")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserInfoAsync(int id)
        {
            var userApp = await userAppMapRepository.GetUserApplicationInfoByIdAsync(id);
            if (userApp == null)
            {
                return NoContent();
            }
            var userAppDto = mapper.Map<UserApplicationMappingDto>(userApp);
            return Ok(userAppDto);
        }

        [HttpGet]
        public ActionResult<List<UserApplicationMapping>> GetAllUsers()
        {
            var userApps = userAppMapRepository.GetAllUserApplication();
            return Ok(userApps);
        }
        [HttpGet("applicationId")]
       [Authorize(Roles = "Admin")]
        public ActionResult<List<UserApplicationMapping>> GetUsersByApplicationId(int applicationId)
        {
            var users = userAppMapRepository.GetUsersByApplicationId(applicationId);
            if(users.Count > 0)
            return Ok(users);
            else
            return NoContent();
        }
        [HttpGet("userId")]
        public ActionResult<List<UserApplicationMapping>> GetApplicationsByUserId(int userId)
        {
            var apps = userAppMapRepository.GetApplicationsByUserId(userId);
            if (apps.Count > 0)
                return Ok(apps);
            else
                return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync(int id, string _userName, UserApplicationMapping _userApp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var userApp = await userAppMapRepository.UpdateUserApplicationAsync(id, _userName, _userApp);
                var userAppDto = mapper.Map<UserApplicationMappingDto>(userApp);
                return Ok(userAppDto);
            }
        }

    }
}


