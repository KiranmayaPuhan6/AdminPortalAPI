using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationAPI.Models.Domain

{
    public class Application
    {
        public int Id { get; set; }

        [Required]
        public string AppName { get; set; }

        [Required]
        public string Url { get; set; }
        [Required]
        public string AppDescription { get; set; }

        //public int Key { get; set; }

       
        public int? UserId { get; set; }
        [AllowNull]
        public string? HealthCheckUrl { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedTime { get; set; }

        [NotMapped]
        public IFormFile? FileUri { get; set; }
        public string? ActualFileUrl { get; set; }

    }


    
}
