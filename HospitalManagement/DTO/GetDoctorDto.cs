using HospitalManagement.DTO.AvailabiltyDto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalManagement.DTO
{
    public class GetDoctorDto
    {
       
        public string Name { get; set; }

        public string Specialization { get; set; }

        public string ContactDetails { get; set; }

        public string DepartmentName { get; set; }
        
        public List<GetAvailabilitySlotDto> AvailabilitySlots { get; set; } = new();
    }
}
