using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> ReadAsync(string EntityID);
        Task<IEnumerable<TEntity>> ReadAll();
        void UpdateAsync(TEntity entity);
        Task DeleteAsync(string entityID);
        int SaveChanges();
    }
}
