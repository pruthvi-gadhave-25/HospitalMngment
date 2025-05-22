using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleBynameAsync(string roleName);
    }
}
