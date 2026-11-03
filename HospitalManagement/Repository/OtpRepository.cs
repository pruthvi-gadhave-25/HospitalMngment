using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class OtpRepository: IOtpRepository
    {
        private readonly AppDbContext _context;

        public OtpRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(UserOtp otp)
        {
            var existingOtps = await _context.UserOtps
                .Where(x => x.UserId == otp.UserId
                         && x.Purpose == otp.Purpose
                         && !x.IsUsed).ToListAsync();
            
            foreach (var existingOtp in existingOtps)
            {
                existingOtp.IsUsed = true;
            }

            await _context.UserOtps.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<UserOtp> GetActiveOtpAsync(int userId, string purpose)
        {
            return await _context.UserOtps
                .Where(x => x.UserId == userId
                         && x.Purpose == purpose
                         && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync() ;
        }

        public async Task UpdateOtpAsync(UserOtp otp)
        {
            await _context.SaveChangesAsync();
        }
    }
}
