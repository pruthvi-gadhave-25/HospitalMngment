using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(UserOtp otp);
        Task<UserOtp> GetActiveOtpAsync(int userId, string purpose);
        Task UpdateOtpAsync(UserOtp otp);
    }
}
