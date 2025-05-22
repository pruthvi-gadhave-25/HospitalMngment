using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<bool> CreatUserAsync(User user)
        {
            var res =  await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
           if(res == null)
            {
                return false;
            }
           return  true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
           var res = await  _context.Users.ToListAsync();
            
            if(res == null)
            {
                return null;
            }

            return res;
        }
    }
}
