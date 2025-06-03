namespace HospitalManagement.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Add(T e);
        Task Update (T e);
        Task<bool> Delete(T e);
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetAll();
        Task SaveAsync();

    }
}
