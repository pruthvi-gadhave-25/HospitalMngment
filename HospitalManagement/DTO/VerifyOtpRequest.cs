namespace HospitalManagement.DTO
{
    public class VerifyOtpRequest
    {
        public int UserId { get; set; }
        public string Purpose { get; set; }
        public string Otp { get; set; }
    }
}
