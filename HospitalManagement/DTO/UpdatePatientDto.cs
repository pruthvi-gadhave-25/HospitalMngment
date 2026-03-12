using System;

namespace HospitalManagement.DTO
{
    public class UpdatePatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
    }
}
