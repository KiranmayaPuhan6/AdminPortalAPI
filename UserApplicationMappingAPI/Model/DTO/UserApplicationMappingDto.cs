

using System.Diagnostics.CodeAnalysis;

namespace UserApplicationMappingAPI.Models.DTO

{
    public class UserApplicationMappingDto
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int UserId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }
        [AllowNull]
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedTime { get; set; }


    }
}
