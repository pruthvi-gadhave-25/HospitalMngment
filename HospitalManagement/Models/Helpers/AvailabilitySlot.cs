namespace HospitalManagement.Models.Helpers
{
    public class AvailabilitySlot
    {
        public int  Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly  EndTime { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
