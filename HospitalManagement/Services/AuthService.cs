using HospitalManagement.Data.UnitOfWork;
using HospitalManagement.DTO;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AuthService : IAuthService 
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterUser(RegisterRequestDto registerRequestDto)
        {
            var isUserExist = await _unitOfWork.UserRepository.GetByEmailAsync(registerRequestDto.Email);
            if (isUserExist != null)
            { 
                _logger.LogError($"Email already exists  : {registerRequestDto.Email}");
                return new AuthResponseDto { Success = false, Message = "Email already exists" };

            }
            
            var role = await _unitOfWork.RoleRepository.GetRoleBynameAsync(registerRequestDto.Role);

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
            
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation($"user Registered Succefully {user.UserName} and role is {user.Role.RoleName}");
            return new AuthResponseDto { Success = true, Message = "user registered Succefully" , Role = user.Role.RoleName};
        }

        public async Task<AuthResponseDto> LoginUserAsync(LoginRequestDto loginRequestDto)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequestDto.Email);

                if (user == null)
                {
                    _logger.LogWarning($"Email does not exist: {loginRequestDto.Email}");
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email"
                    };
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                    loginRequestDto.Password,
                    user.PasswordHash
                );

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Password does not match");
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Password does not match"
                    };
                }

                var token = _jwtTokenGenerator.GenerateJwtToken(user);

                _logger.LogInformation(
                    $"User login successful: {user.UserName}, Role: {user.Role.RoleName}"
                );

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Role = user.Role.RoleName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login exception");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Internal server error"
                };
            }
        }
    }
}
