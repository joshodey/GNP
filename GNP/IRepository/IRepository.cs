namespace GNP.IRepository
{
    public interface IRepository<T, Tkey> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<T> GetAsync(Tkey Id);
        IQueryable<T> GetAllAsync();
    }
}
