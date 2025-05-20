using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Patient
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "required name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "required mobileNo")]
        public string Mobile{ get; set; }

        [Required(ErrorMessage = "required Email id")]
        public string Email  { get; set; }

        [Required(ErrorMessage = "required Gender)")]
        public  string Gender { get; set; }

        [Required(ErrorMessage = "required Date of  birth")]
        public DateTime Dob { get; set; }


        public ICollection<Appointment>? Appointments { get; set; }


    }
}
