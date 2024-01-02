using UserAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace UserAPI.Data
{
    public class UserAPIDB:DbContext
    {
        public UserAPIDB(DbContextOptions options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
