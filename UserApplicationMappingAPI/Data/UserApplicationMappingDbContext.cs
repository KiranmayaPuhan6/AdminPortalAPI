using Microsoft.EntityFrameworkCore;
using UserApplicationMappingAPI.Model.Domain;

namespace UserApplicationMappingAPI.Data
{
    public class UserApplicationMappingDbContext : DbContext
    {
        public UserApplicationMappingDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<UserApplicationMapping> UserAssignedApplications { get; set; }

    }
}
