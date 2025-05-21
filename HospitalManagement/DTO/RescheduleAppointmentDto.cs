namespace HospitalManagement.DTO
{
    public class RescheduleAppointmentDto
    {
       public  int AppointmentId { get; set; }
        public DateTime NewDate { get; set; }

        public TimeOnly NewTime {  get; set; }
    }
}
