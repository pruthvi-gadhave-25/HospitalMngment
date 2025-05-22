using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        public string  PasswordHash { get; set; }

        public Role? Role { get; set; }

        public int RoleId { get; set; }
    }
}
