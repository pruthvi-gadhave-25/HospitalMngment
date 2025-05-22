using HospitalManagement.DTO;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        
        }

        public async Task<List<GetUserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();

            var userDto = users.Select(u => new GetUserDto
            {
                UserName = u.UserName,
                Email = u.Email,
                RoleName = u.Role?.RoleName,
            }).ToList() ?? new ();

            return userDto ;
        }
    }

}
