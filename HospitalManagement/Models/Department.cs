using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        public string Name { get; set; }

        ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
