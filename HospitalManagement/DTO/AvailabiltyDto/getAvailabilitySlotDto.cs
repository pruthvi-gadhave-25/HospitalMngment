namespace HospitalManagement.DTO.AvailabiltyDto
{
    public class GetAvailabilitySlotDto
    {
        public DayOfWeek DayofWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
