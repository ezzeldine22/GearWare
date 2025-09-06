using CleanArchitecture.Core.Interfaces;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private EcommerceDbContext _context;
        private DbSet<TEntity> _entity;

        public Repository(EcommerceDbContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> ReadAsync(string EntityID)
        {
            return await _entity.FindAsync(EntityID);
        }

        public async Task<IEnumerable<TEntity>> ReadAll()
        {
            return await _entity.ToListAsync();
        }

        public void UpdateAsync(TEntity entity)
        {
            _entity.Attach(entity);
        }

        public async Task DeleteAsync(string entityID)
        {
            var oData = await _entity.FindAsync(entityID);
            _entity.Remove(oData);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
