using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models.DTO
{
    public class RegisterUser
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
        public string AppRole { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public long Contact { get; set; }
    

    }
}
