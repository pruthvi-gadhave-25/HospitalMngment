namespace HospitalManagement.DTO
{
    public class AddLeaveDto
    {
        public int DoctorId { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
    }
}
