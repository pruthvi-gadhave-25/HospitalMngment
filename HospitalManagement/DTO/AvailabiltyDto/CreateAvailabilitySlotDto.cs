namespace HospitalManagement.DTO.AvailabiltyDto
{
    public class CreateAvailabilitySlotDto
    {
        public int DoctorId { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
