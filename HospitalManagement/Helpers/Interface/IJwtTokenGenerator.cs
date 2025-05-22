using HospitalManagement.Models;

namespace HospitalManagement.Helpers.Interface
{
    public interface IJwtTokenGenerator 
    {
        string GenerateJwtToken( User user);
    }
}
