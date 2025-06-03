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
    }
}
