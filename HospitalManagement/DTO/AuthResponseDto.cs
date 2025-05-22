using HospitalManagement.Models;

namespace HospitalManagement.DTO
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message {  get; set; }

        public string Token { get; set; }
        public string Role { get; set; }
    }
}
