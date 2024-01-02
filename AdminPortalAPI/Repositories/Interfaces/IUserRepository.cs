using UserAPI.Models.Domain;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
       
        Task<User> GetUserInfoAsync(int id);
        Task<User> GetUserByNameAsync(string name);
        Task<User> DeleteUserAsync(int id);
        Task<User> UpdateUserAsync(int id, User user,string useName);
        List<User> GetUsers();
        Task<User> GetRoleById(int id);

        Task<User> GetUserByEmailAsync(string email);
        Task<User> ResetPasswordTokenAsync(string email,int token,DateTime tokenExpiry);
        Task<User> GetUserByTokenAsync(int token);
        Task<User> ChangePasswordAsync(ResetPassword data);

    }
}
