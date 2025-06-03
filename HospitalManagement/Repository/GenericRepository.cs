using HospitalManagement.Data;
using HospitalManagement.Interface;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;

namespace HospitalManagement.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        protected AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _appDbContext = context;
            _dbSet = _appDbContext.Set<T>();
        }
        public async Task<bool> Add(T e)
        {
           var res =   await _dbSet.AddAsync(e);
            return true;
        }

        public async Task<bool> Delete(T e)
        {
            _dbSet.Remove(e);
            return true;
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Update(T e)
        {
            _dbSet.Update(e);
        }
        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
           
        }
    }
}
