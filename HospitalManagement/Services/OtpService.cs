using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagement.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly byte[] _secretKey;

        public OtpService(IOtpRepository otpRepository, IConfiguration configuration)
        {
            _otpRepository = otpRepository;

            var secret = configuration["Otp:HmacKey"];

            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("OTP HMAC key is not configured.");

            _secretKey = Convert.FromBase64String(secret);

            if (_secretKey.Length < 32)
                throw new InvalidOperationException("OTP HMAC key must contain at least 32 bytes.");
        }


        public async Task GenerateOtpAsync(int userId, string purpose)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            if (purpose != "Registration" && purpose != "Login")
                throw new ArgumentException("Invalid OTP purpose.");

            string otp = RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");

            string otpHash;

            using (var hmac = new HMACSHA256(_secretKey))
            {
                var data = Encoding.UTF8.GetBytes(
                    $"{userId}:{purpose}:{otp}");

                otpHash = Convert.ToBase64String(
                    hmac.ComputeHash(data));
            }

            var userOtp = new UserOtp
            {
                UserId = userId,
                Purpose = purpose,
                OtpHash = otpHash,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow,
                IsUsed = false,
                FailedAttempts = 0
            };

            await _otpRepository.SaveOtpAsync(userOtp);

            // Later: pass OTP to EmailService.
            // Never log or return OTP in production.
        }


        public async Task<bool> VerifyOtpAsync(int userId, string purpose, string otp)
        {
            if (userId <= 0 || (purpose != "Registration" && purpose != "Login") ||
                string.IsNullOrWhiteSpace(otp) ||
                otp.Length != 6 ||
                !otp.All(char.IsAsciiDigit) )
            {
                return false;
            }

            var userOtp = await _otpRepository.GetActiveOtpAsync(userId, purpose);

            if (userOtp == null)
                return false;

            if (userOtp.ExpiryTime <= DateTime.UtcNow)
                return false;

            if (userOtp.FailedAttempts >= 5)
                return false;

            string otpHash;

            using (var hmac = new HMACSHA256(_secretKey))
            {
                var data = Encoding.UTF8.GetBytes($"{userId}:{purpose}:{otp}");

                otpHash = Convert.ToBase64String(hmac.ComputeHash(data));
            }

            bool isValid = CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(userOtp.OtpHash),Convert.FromBase64String(otpHash));

            if (!isValid)
            {
                userOtp.FailedAttempts++;
                await _otpRepository.UpdateOtpAsync(userOtp);
                return false;
            }

            userOtp.IsUsed = true;
            await _otpRepository.UpdateOtpAsync(userOtp);

            return true;
        }


    }
}

