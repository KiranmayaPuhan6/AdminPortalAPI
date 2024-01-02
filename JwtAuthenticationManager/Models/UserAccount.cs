using System.ComponentModel.DataAnnotations;

namespace JwtAuthenticationManager.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public long Contact { get; set; }
    }
}
