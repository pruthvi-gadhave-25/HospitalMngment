using HospitalManagement.DTO;

namespace HospitalManagement.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUser(RegisterRequestDto registerRequestDto);

        Task<AuthResponseDto> LoginUserASync(LoginRequestDto loginRequestDto);  

    }
}
