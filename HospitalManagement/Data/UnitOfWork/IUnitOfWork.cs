using HospitalManagement.Repository.Interface;
using System.Threading.Tasks;

namespace HospitalManagement.Data.UnitOfWork
{
    public interface IUnitOfWork  
    {
        IDepartmentRepository DepartmentRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        IPatientRepository PatientRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ILeaveRepository LeaveRepository { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
