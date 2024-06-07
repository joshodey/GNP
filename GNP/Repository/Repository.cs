using GNP.Context;
using GNP.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GNP.Repository
{
    public class Repository<T, Tkey> : IRepository<T, Tkey> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<T> CreateAsync(T entity)
        {
            var result = await _context.AddAsync<T>(entity);

            return await SaveChangesAsync()? result.Entity : null;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove<T>(entity);

            return await SaveChangesAsync();
        }

        public IQueryable<T> GetAllAsync()
        {
            return  _context.Set<T>().AsQueryable<T>();
        }

        public async Task<T> GetAsync(Tkey Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _context.Update<T>(entity);

            return await SaveChangesAsync() ? result.Entity : null;
        }

        #region Helper

        private async Task<bool> SaveChangesAsync()
        {
            int count = await _context.SaveChangesAsync();

            return count > 0;
        }

        #endregion
    }
}
