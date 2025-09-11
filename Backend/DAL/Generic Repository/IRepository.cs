using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> ReadAsync(string EntityID);
        IQueryable<TEntity> ReadAllAsync();
        void UpdateAsync(TEntity entity);
        Task DeleteAsync(string entityID);

        Task<IEnumerable<TResult>> Select<TSource, TResult>(Expression<Func<TSource, TResult>> selector) where TSource : class;
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        int SaveChanges();
    }
}
