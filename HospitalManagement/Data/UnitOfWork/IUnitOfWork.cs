using HospitalManagement.Repository.Interface;

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
     
    }
}
