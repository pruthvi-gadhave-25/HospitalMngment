using HospitalManagement.Data;
using HospitalManagement.Interface;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;

namespace HospitalManagement.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext _context;
        private readonly DbSet<T>   _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T e)
        {
            await _dbSet.AddAsync(e);                           
        }

        public async Task Delete(T e)
        {
             _dbSet.Remove(e);
        }

        public  async Task<T> GetById(object id)
        {
            return  await  _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public  async Task Update(T e)
        {
            _dbSet.Update(e);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
