using Microsoft.EntityFrameworkCore;
using ApplicationAPI.Data;
using ApplicationAPI.Models.Domain;
using ApplicationAPI.Repositories.Interfaces;

namespace ApplicationAPI.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ApplicationRepository(ApplicationDbContext _applicationDbContext)
        {
            this.applicationDbContext = _applicationDbContext;
        }
        public async Task<List<Application>> GetApplications()
        {
            return applicationDbContext.Applications.ToList();
        }
        public async Task<Application> DeleteApplicationAsync(int id)
        {
            var application=await applicationDbContext.Applications.FirstOrDefaultAsync(x => x.Id == id);
            if (application == null)
            {
                return null;
            }
            else
            {
                applicationDbContext.Remove(application);
                await applicationDbContext.SaveChangesAsync();
            }
            return application;
        }

        public async Task<Application> GetApplicationInfoAsync(int id)
        {
            return await applicationDbContext.Applications.FirstOrDefaultAsync(x => x.Id==id);
        }

       

        public async Task<Application> RegisterApplicationAsync(Application application,string userName)
        {
            await applicationDbContext.AddAsync(application);
            await applicationDbContext.SaveChangesAsync();
            return application;
        }

        public async Task<Application> UpdateApplicationAsync(int id, Application _application, string _userName)
        {
           var app= await applicationDbContext.Applications.Where(x=>x.Id== id).SingleAsync();
            app.AppName=_application.AppName;
            app.HealthCheckUrl = _application.HealthCheckUrl;
            app.AppDescription= _application.AppDescription;
            app.Url = _application.Url;
            //app.Key= _application.Key;
            app.LastUpdatedBy= _userName;
            app.LastUpdatedTime= DateTime.Now;

            applicationDbContext.Applications.Update(app);
            await applicationDbContext.SaveChangesAsync();
            return app;
        }

        public async Task<Application> GetApplicationByNameAsync(string name)
        {
            return await applicationDbContext.Applications.FirstOrDefaultAsync(x => x.AppName==name);
        }

        public async Task<List<Application>> DeleteApplicationsByUsernameAsync(string userName)
        {
           var apps = await GetApplications();
            var createdApps = new List<Application>();  
            if(apps.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (var app in apps)
                {
                   if( app.CreatedBy == userName)
                    {
                         createdApps.Add(app);
                    }
                }
                if(createdApps.Count > 0)
                {
                    applicationDbContext.RemoveRange(createdApps);
                    await applicationDbContext.SaveChangesAsync();
                    return createdApps;
                }
                return null;
            }
        }
    }
}

