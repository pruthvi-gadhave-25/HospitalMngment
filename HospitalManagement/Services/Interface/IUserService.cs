using HospitalManagement.DTO;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IUserService
    {
        Task<List<GetUserDto>> GetUsersAsync();
       
    }
}
