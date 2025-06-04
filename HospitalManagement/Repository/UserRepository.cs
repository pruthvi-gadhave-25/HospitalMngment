using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
          
        }
        public async Task<List<User>> GetUsersAsync()
        {
            //var res = await (from s in appDbContext.Users
            //                 select new
            //                 {
            //                     s.Id,
            //                     s.Email
            //                 }).ToListAsync(); //use this  when return specific data only            
            var res =  await _appDbContext.Users
                .Include(s =>s.Role)
                .ToListAsync();                

            return res;
        }

        public async Task<bool> CreatUserAsync(User user)
        {
            var res = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            if (res == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _appDbContext.Users
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
    }
}
