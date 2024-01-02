using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationAPI.Models.DTO

{
    public class ApplicationDto
    {

        public int Id { get; set; }

       
        public string AppName { get; set; }

        public string Url { get; set; }
        
        public string AppDescription { get; set; }

        //public int Key { get; set; }

       
        public int? UserId { get; set; }
      
        public string? HealthCheckUrl { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }


        public IFormFile FileUri { get; set; }
        public string? ActualFileUrl { get; set; }

    }
}
