namespace HospitalManagement.Services.Interface
{
    public interface IOtpService
    {
        Task GenerateOtpAsync(int userId, string purpose);
        Task<bool> VerifyOtpAsync(int userId, string purpose, string otp);

    }
}
