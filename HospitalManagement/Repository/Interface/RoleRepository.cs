
using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository.Interface
{
    public class RoleRepository : IRoleRepository
    {   
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Role> GetRoleBynameAsync(string roleName)
        {
            var role = await  _context.Roles.FirstOrDefaultAsync(  r => r.RoleName == roleName);   
            if (role == null)
            {
                return null;
            }
            return role;
        }

        public async Task<bool> CreatUserAsync(User user)
        {
            var res = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            if (res == null)
            {
                return false;
            }
            return true;
        }
    }
}
