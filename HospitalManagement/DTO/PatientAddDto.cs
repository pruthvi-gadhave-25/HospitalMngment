using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.DTO
{
    public class PatientAddDto
    {     
        public string Name { get; set; }
     
        public string Mobile { get; set; }
     
        public string Email { get; set; }
     
        public string Gender { get; set; }

        
        public DateTime Dob { get; set; }
    }
}
