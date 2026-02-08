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

        private IDepartmentRepository _departmentRepository;
        private IDoctorRepository _doctorRepository;
        private IPatientRepository _patientRepository;
        private IAppointmentRepository _appointmentRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private ILeaveRepository _leaveRepository;

        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return _departmentRepository ??= new DepartmentRepository(_context);
            }
        }

        public IDoctorRepository DoctorRepository
        {
            get
            {
                return _doctorRepository ??= new DoctorRepository(_context);
            }
        }

        public IPatientRepository PatientRepository
        {
            get
            {
                return _patientRepository ??= new PatientRepository(_context);
            }
        }

        public IAppointmentRepository AppointmentRepository
        {
            get
            {
                return _appointmentRepository ??= new AppointmentRepository(_context);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new RoleRepository(_context);
            }
        }

        public ILeaveRepository LeaveRepository
        {
            get
            {
                return _leaveRepository ??= new LeaveManagementRepository(_context);
            }
        }

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
                await _transaction?.CommitAsync();
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
                await _transaction?.RollbackAsync();
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
