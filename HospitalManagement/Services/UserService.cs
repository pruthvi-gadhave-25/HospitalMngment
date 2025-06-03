using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Interface;
using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
       
        public UserService(UserRepository userRepository)
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
            }).ToList() ?? new();
            

            return userDto;
        }
        public async Task CreateUserAsync(string name , string email , string role)
        {
            var data = new User();
            data.UserName = name;
            data.Email = email;

            await _userRepository.Add(data);
            await _userRepository.SaveAsync();
        }

        public async Task<List<User>> GetList()
        {
            var data = await _userRepository.GetUsersAsync(); //from repository
            return data;
        }
    }

}
