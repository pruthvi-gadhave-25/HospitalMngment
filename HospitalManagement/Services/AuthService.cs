using HospitalManagement.DTO;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AuthService : IAuthService 
    {   
        private readonly UserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserRepository userRepository , IRoleRepository roleRepository , IJwtTokenGenerator jwtTokenGenerator,ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterUser(RegisterRequestDto registerRequestDto)
        {
            var isUserExist = await    _userRepository.GetByEmailAsync(registerRequestDto.Email);
            if (isUserExist != null)
            { 
                _logger.LogError($"Email already exists  : {registerRequestDto.Email}");
                return new AuthResponseDto { Success = false, Message = "Email already exists" };

            }
            var role = await _roleRepository.GetRoleBynameAsync(registerRequestDto.Role);

            if (role == null)
            {
                _logger.LogError($"Role in inavlid: {registerRequestDto.Role}");
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
            _logger.LogInformation($"user Registered Succefully {user.UserName} and role is {user.Role.RoleName}");
            return new AuthResponseDto { Success = true, Message = "user registered Succefully" , Role = user.Role.RoleName};
        }

        public async  Task<AuthResponseDto> LoginUserASync(LoginRequestDto loginRequestDto)
        {
            try
            {
                User user =await   _userRepository.GetByEmailAsync(loginRequestDto.Email);
                if (user == null)
                {
                    _logger.LogError($"\n==========\n Email  does not exists  : {loginRequestDto.Email}\n =============\n");
                    return new AuthResponseDto { Message ="invalid email" , Success=false};
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequestDto.Password);

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    _logger.LogError($"Password does not match");
                    return new AuthResponseDto { Message = "passowrd does not match", Success = false };
                }
               var token =  _jwtTokenGenerator.GenerateJwtToken(user);

                _logger.LogInformation($"\n========\n user Loin Succefully {user.UserName} and role is {user.Role} \n=============\n");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "token created ",
                    Token = token,
                    Role = user.Role.RoleName
                };
            }catch (Exception ex)
            {
                _logger.LogError($"exception Occurs : {ex.Message}");
                return new AuthResponseDto { Success = false,   Message=ex.Message };   
            }
        }

        
    }
}
