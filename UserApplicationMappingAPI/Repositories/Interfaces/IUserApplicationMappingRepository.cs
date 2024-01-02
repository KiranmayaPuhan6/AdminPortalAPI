using UserApplicationMappingAPI.Model.Domain;

namespace UserApplicationMappingAPI.Repositories.Interfaces
{
    public interface IUserApplicationMappingRepository
    {
        Task<UserApplicationMapping> AddUserApplicationAsync(string userName,UserApplicationMapping userApplication);
       
        Task<UserApplicationMapping> GetUserApplicationInfoByIdAsync(int id);
        
        Task<UserApplicationMapping> DeleteUserApplicationAsync(int id);
        Task<UserApplicationMapping> UpdateUserApplicationAsync(int id, string userName,UserApplicationMapping userApplication);
        List<UserApplicationMapping> GetAllUserApplication();
        List<UserApplicationMapping> GetUsersByApplicationId(int applicationId);
        List<UserApplicationMapping> GetApplicationsByUserId(int userId);
    }
}
