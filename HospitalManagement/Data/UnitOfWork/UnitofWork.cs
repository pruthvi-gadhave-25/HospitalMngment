using HospitalManagement.Data;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace HospitalManagement.Data.UnitOfWork
{
    public class UnitofWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        private IPatientRepository _patientRepository;
        private IDepartmentRepository _departmentRepository;
        private IDoctorRepository _doctorRepository;
        private IAppointmentRepository _appointmentRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private ILeaveRepository _leaveRepository;

        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }

        public IPatientRepository PatientRepository =>
            _patientRepository ??= new PatientRepository(_context);

        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ??= new DepartmentRepository(_context);

        public IDoctorRepository DoctorRepository =>
            _doctorRepository ??= new DoctorRepository(_context);

        public IAppointmentRepository AppointmentRepository =>
            _appointmentRepository ??= new AppointmentRepository(_context);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public IRoleRepository RoleRepository =>
            _roleRepository ??= new RoleRepository(_context);

        public ILeaveRepository LeaveRepository =>
            _leaveRepository ??= new LeaveManagementRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
