using System.Diagnostics.CodeAnalysis;

namespace UserApplicationMappingAPI.Model.Domain
{
    public class UserApplicationMapping
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int UserId { get; set; }
        [AllowNull]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }
        [AllowNull]
        public string? LastUpdatedBy { get; set; }
        [AllowNull]
        public DateTime? LastUpdatedTime { get; set; }
    }

}
