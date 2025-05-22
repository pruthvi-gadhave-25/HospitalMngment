using HospitalManagement.DTO;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AuthService : IAuthService 
    {   

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;


        public AuthService(IUserRepository userRepository , IRoleRepository roleRepository , IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterUser(RegisterRequestDto registerRequestDto)
        {
            var isUserExist = await    _userRepository.GetByEmailAsync(registerRequestDto.Email);
            if (isUserExist != null)
            {
                return new AuthResponseDto { Success = false, Message = "Email already exists" };

            }
            var role = await _roleRepository.GetRoleBynameAsync(registerRequestDto.Role);

            if (role == null)
            {
                return new AuthResponseDto { Success = false, Message = "Invalid role" };

            }

            var user = new User
            {
                UserName = registerRequestDto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password),
                Email = registerRequestDto.Email,               
                RoleId = role.Id,
                Role =  role
            };
            await _userRepository.CreatUserAsync(user);
            return new AuthResponseDto { Success = true, Message = "user registered Succefully" , Role = user.Role.RoleName};
        }

        public async  Task<AuthResponseDto> LoginUserASync(LoginRequestDto loginRequestDto)
        {
            try
            {
                User user =await   _userRepository.GetByEmailAsync(loginRequestDto.Email);
                if (user == null)
                {
                    return new AuthResponseDto { Message ="invalid email" , Success=false};
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequestDto.Password);

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return new AuthResponseDto { Message = "passowrd does not match", Success = false };
                }
               var token =  _jwtTokenGenerator.GenerateJwtToken(user);


                return new AuthResponseDto
                {
                    Success = false,
                    Message = "token created ",
                    Token = token,
                    Role = user.Role.RoleName
                };
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AuthResponseDto { Success = false,   Message=ex.Message };   
            }
        }

        
    }
}
