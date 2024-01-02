using ApplicationAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApplicationAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Application> Applications { get; set; }
    }
}
