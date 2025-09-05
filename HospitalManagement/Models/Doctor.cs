using HospitalManagement.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Availability slot is required.")]
        public string AvailabilitySlot { get; set; }

        [Required(ErrorMessage = "Contact details are required.")]
        public string ContactDetails { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        public int DepartmentId { get; set; }

        //public string  Address { get; set; } = string.Empty;
    
        public virtual  Department? Department { get; set; } //foreign


        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<AvailabilitySlot>? AvailabilitySlots { get; set; }


    }
}
