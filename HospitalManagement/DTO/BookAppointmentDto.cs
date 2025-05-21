namespace HospitalManagement.DTO
{
    public class BookAppointmentDto
    {
        public int PatientID { get; set; }
        public int DoctorId { get; set; }
        public string Diagnoasis { get; set; }

        public string Medications { get; set; }

        public string Treatement { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }
    }
}
