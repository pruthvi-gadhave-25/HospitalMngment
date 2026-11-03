namespace HospitalManagement.DTO
{
    public class GenerateOtpRequest
    {
        public int UserId { get; set; }
        public string Purpose { get; set; } = string.Empty;
    }
}
