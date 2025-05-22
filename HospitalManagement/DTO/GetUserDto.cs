using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.DTO
{
    public class GetUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }
    }
}
