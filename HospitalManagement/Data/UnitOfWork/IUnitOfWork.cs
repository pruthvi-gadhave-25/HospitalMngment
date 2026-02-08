using HospitalManagement.Repository.Interface;
using HospitalManagement.Repository;

namespace HospitalManagement.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable 
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
