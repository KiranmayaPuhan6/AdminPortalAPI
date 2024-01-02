using ApplicationAPI.Models.Domain;
using ApplicationAPI.Repositories;
using ApplicationAPI.Repositories.Interfaces;
using ApplicationAPI.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase


    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment environment;

        public ApplicationController(IApplicationRepository _applicationRepository, IMapper _mapper, IWebHostEnvironment _environment)
        {
            this.applicationRepository = _applicationRepository;
            this.mapper = _mapper;
            environment = _environment;
        }
        [HttpGet]
       [Authorize(Roles ="Admin,User,Owner")]
        public async Task<IActionResult> GetApplicationsAsync()
        {
            var allApp = await applicationRepository.GetApplications();
            if (allApp.Count == 0)
            {
                return NoContent();
            }

            var quesDto = mapper.Map<List<ApplicationDto>>(allApp);
            return Ok(quesDto);
        }

        [HttpPost]
     [Authorize(Roles ="Admin,Owner")]
        public async Task<IActionResult> RegisterApplicationAsync([FromForm]Application application,string _userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (application == null)
            {
                return NotFound();
            }
            else
            {
                string path;
                if (application.FileUri != null)
                {
                    path = await UploadImage(application.FileUri);
                    if(path == "Not a valid type")
                    {
                        return BadRequest($"Not a valid type");
                    }
                }
                else
                {
                    path = null;
                }
                var appModel = new Application()
                {
                    AppName = application.AppName,
                    Url = application.Url,
                    UserId = application.UserId,
                    AppDescription = application.AppDescription,
                    //Key = application.Key,
                    HealthCheckUrl = application.HealthCheckUrl,
                    CreatedBy = _userName,
                    CreatedTime = DateTime.Now,
                    ActualFileUrl = path
                };
                application = await applicationRepository.RegisterApplicationAsync(appModel, _userName);
                var appDto = mapper.Map<ApplicationDto>(application);
                return Ok(appDto);
            }
        }

        [HttpDelete("Id")]
        [Authorize(Roles ="Admin,Owner")]
        public async Task<IActionResult> DeleteApplicationAsync(int id)
        {
            var app = await applicationRepository.DeleteApplicationAsync(id);
            if (app == null)
            {
                return BadRequest();
            }
            else
            {
                DeleteImage(app.ActualFileUrl);
                var appDto=mapper.Map<ApplicationDto>(app);
                return Ok(appDto);
            }
        }

        [HttpPut]
        [Authorize(Roles ="Admin,Owner")]

        public async Task<IActionResult> UpdateApplicationAsync(int id,Application application, string _userName)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState); 
            }
            var app=await applicationRepository.UpdateApplicationAsync(id, application,_userName);
            if (app == null)
            {
                return NoContent();
            }
            else
            {
                var appDto = mapper.Map<ApplicationDto>(app);
                return Ok(appDto);
            }

        }

        [HttpGet("Id")]
       [Authorize(Roles ="Admin,User,Owner")]
        
        public async Task<IActionResult> GetApplicationInfoAsync(int id)
        {
            var app = await applicationRepository.GetApplicationInfoAsync(id);
            if (app == null)
            {
                return NoContent();
            }
            var appDto = mapper.Map<ApplicationDto>(app);
            return Ok(appDto);
     
        }
        [HttpGet("AppName")]

        public async Task<IActionResult> GetApplicationByNameAsync(string name)
        {
            var appName=await applicationRepository.GetApplicationByNameAsync(name);
            if (appName == null)
            {
                return NoContent();
            }
            var appNameDto=mapper.Map<ApplicationDto>(appName);
            return Ok(appNameDto);
        }

        [HttpDelete("userName")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> DeleteApplicationsByUsernameAsync(string userName)
        {
            var createdApps = await applicationRepository.DeleteApplicationsByUsernameAsync(userName); 
            if (createdApps == null)
            {
                return NoContent();
            }
            foreach(var app in createdApps)
            {
                DeleteImage(app.ActualFileUrl);
            }
            return Ok(createdApps);
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
    }
}