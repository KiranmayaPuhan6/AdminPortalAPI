using ApplicationAPI.Models.Domain;

namespace ApplicationAPI.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application> RegisterApplicationAsync(Application application,string userName);
        Task<Application> GetApplicationInfoAsync(int id);
        Task<Application> GetApplicationByNameAsync(string name);
        Task<Application> DeleteApplicationAsync(int id);
        Task<Application> UpdateApplicationAsync(int id, Application application, string _userName);
        Task<List<Application>> DeleteApplicationsByUsernameAsync(string userName);
        Task <List<Application>> GetApplications();


    }
}
