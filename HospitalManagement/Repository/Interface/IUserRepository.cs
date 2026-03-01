using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Models;
using HospitalManagement.Interface;

namespace HospitalManagement.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetUsersAsync();
        Task<bool> CreatUserAsync(User user);
    }
}
