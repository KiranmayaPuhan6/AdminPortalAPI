using UserAPI.Repositories.Interfaces;
using UserAPI.Data;
using UserAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models.Domain;

namespace UserAPI.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserAPIDB userDbContext;
        public UserRepository(UserAPIDB _userDbContext) 
        {
            userDbContext = _userDbContext;
        }       

        public async Task<User> RegisterUserAsync(User user)
        {
            await userDbContext.AddAsync(user);
            await userDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserInfoAsync(int id)
        {
            var user = await userDbContext.Users.FirstOrDefaultAsync(x => x.Id==id);
            if(user==null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            var user = await userDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }
            else
            {
                userDbContext.Remove(user);
                await userDbContext.SaveChangesAsync();
            }
            return user;
        }

        public List<User> GetUsers()
        {
            return  userDbContext.Users.ToList();
        }

        public async Task<User> UpdateUserAsync(int id, User _user , string useName)
        {
            var user = await userDbContext.Users.Where(x => x.Id == id).SingleAsync();



            user.UserName = _user.UserName;
            user.Password = _user.Password;
            user.FirstName = _user.FirstName;
            user.LastName = _user.LastName;
            user.Email = _user.Email;
            user.Contact = _user.Contact;
            user.Role = _user.Role;
            user.AppRole = _user.AppRole;
            user.AppId = _user.AppId;
            user.LastUpdatedBy = useName;
            user.LastUpdatedTime= DateTime.Now;
            



            userDbContext.Users.Update(user);
            await userDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await userDbContext.Users.FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<User> GetRoleById(int id)
        {
            return await userDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = GetUsers();
            foreach (var user in users)
            {
               if( user.Email == email)
                    return user;
            }
            return null;
        }

        public async Task<User> GetUserByTokenAsync(int token)
        {
            var users = GetUsers();
            foreach (var user in users)
            {
                if (user.PasswordResetToken == token)
                    return user;
            }
            return null;
        }

        public async Task<User> ResetPasswordTokenAsync(string email,int token, DateTime tokenExpiry)
        {

            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            user.PasswordResetToken = token;
            user.ResetTokenExpires = tokenExpiry;
            await userDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangePasswordAsync(ResetPassword data)
        {
            var user = await GetUserByTokenAsync(data.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return null;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await userDbContext.SaveChangesAsync();
            return user;
        }

    }
}

