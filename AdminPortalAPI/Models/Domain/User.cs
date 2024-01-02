﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15,MinimumLength =3)]        
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

        [Required,DataType(DataType.PhoneNumber)]
        public long? Contact { get; set; }
        public int? AppId { get; set; }
        public string? CreatedBy { get; set; } // user_id by default
        public DateTime? CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set;} //should be string or int 
        public DateTime? LastUpdatedTime { get; set;}
        [NotMapped]
        public IFormFile? FileUri { get; set; }
        public string? ActualFileUrl { get; set; }
        public int? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

    }
}
