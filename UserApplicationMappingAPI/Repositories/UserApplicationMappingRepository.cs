using UserApplicationMappingAPI.Repositories.Interfaces;
using UserApplicationMappingAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserApplicationMappingAPI.Model.Domain;
using System;


namespace UserApplicationMappingAPI.Repositories
{
    public class UserApplicationMappingRepository: IUserApplicationMappingRepository
    {
        private readonly UserApplicationMappingDbContext userApplicationDbContext;
        public UserApplicationMappingRepository(UserApplicationMappingDbContext _userApplicationDbContext) 
        {
            userApplicationDbContext = _userApplicationDbContext;
        }       

        public async Task<UserApplicationMapping> AddUserApplicationAsync(string userName,UserApplicationMapping userApplication)
        {
            await userApplicationDbContext.AddAsync(userApplication);
            await userApplicationDbContext.SaveChangesAsync();
            return userApplication;
        }

        public async Task<UserApplicationMapping> GetUserApplicationInfoByIdAsync(int id)
        {
            return await userApplicationDbContext.UserAssignedApplications.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<UserApplicationMapping> DeleteUserApplicationAsync(int id)
        {
            var userApplication = await userApplicationDbContext.UserAssignedApplications.FirstOrDefaultAsync(x => x.Id == id);
            if (userApplication == null)
            {
                return null;
            }
            else
            {
                userApplicationDbContext.Remove(userApplication);
                await userApplicationDbContext.SaveChangesAsync();
            }
            return userApplication;
       
        }

        public List<UserApplicationMapping> GetAllUserApplication()
        {
            return userApplicationDbContext.UserAssignedApplications.ToList();
        }

        public async Task<UserApplicationMapping> UpdateUserApplicationAsync(int id, string userName, UserApplicationMapping _userApp)
        {
            var userApp = await userApplicationDbContext.UserAssignedApplications.Where(x => x.Id == id).SingleAsync();

            userApp.AppId=_userApp.AppId;
            userApp.UserId= _userApp.UserId;
            userApp.LastUpdatedBy = userName;
            userApp.LastUpdatedTime = DateTime.Now;

            userApplicationDbContext.UserAssignedApplications.Update(userApp);
            await userApplicationDbContext.SaveChangesAsync();
            return userApp;
        }

        public List<UserApplicationMapping> GetUsersByApplicationId(int applicationId)
        {
            var users = userApplicationDbContext.UserAssignedApplications.Where(x => x.AppId == applicationId).ToList();
            return users;
        }

        public List<UserApplicationMapping> GetApplicationsByUserId(int userId)
        {
            var apps = userApplicationDbContext.UserAssignedApplications.Where(x => x.UserId == userId).ToList();
            return apps;
        }
    }
}

