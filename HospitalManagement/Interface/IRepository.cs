namespace HospitalManagement.Interface
{
    public interface IRepository<T> where T : class
    {
        Task Add(T e);
        Task Update (T e);
        Task Delete(T e);
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetAll();
        Task SaveAsync();

    }
}
