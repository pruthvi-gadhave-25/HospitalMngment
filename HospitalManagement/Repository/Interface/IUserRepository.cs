using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> CreatUserAsync(User user);
        Task<List<User>> GetUsersAsync();


    }
}
