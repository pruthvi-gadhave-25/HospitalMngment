namespace HospitalManagement.Models
{
    public class UserOtp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OtpHash { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
