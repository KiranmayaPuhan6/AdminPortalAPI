using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.Domain
{
    public class ResetPassword
    {
        [Required]
        public int Token { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
