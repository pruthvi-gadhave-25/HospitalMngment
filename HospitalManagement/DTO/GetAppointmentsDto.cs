using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.DTO
{
    public class GetAppointmentsDto
    {
        public string Diagnoasis { get; set; }

        public string Medications { get; set; }

        public string Treatement { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }

        public string   DoctorName { get; set; }
        public string DepartmentName { get; set; }
        public AppointmentStatus Status { get; set; }

    }

}
