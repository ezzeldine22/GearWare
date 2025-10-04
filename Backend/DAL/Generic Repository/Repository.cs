﻿using CleanArchitecture.Core.Interfaces;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<TEntity> ReadById(int RowID)
        {
            return await _entity.FindAsync(RowID);
        }
        public IQueryable<TEntity> ReadAll()
        {
            return _entity.AsNoTracking();
        }
        public void UpdateAsync(TEntity entity)
        {
            _entity.Attach(entity);
        }

        public async Task DeleteAsync(int entityID)
        {
            var oData = await _entity.FindAsync(entityID);
            _entity.Remove(oData);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) 
        {
           return await _entity.FirstOrDefaultAsync(predicate) ;
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.Where(predicate);
        }



        public async Task<IEnumerable<TResult>> Select<TSource, TResult>(
        Expression<Func<TSource, TResult>> selector
        ) where TSource : class => await _context.Set<TSource>()
                         .Select(selector)
                         .ToListAsync();
    }
}
