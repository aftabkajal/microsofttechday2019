using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ToDo.Core.Interfaces;
using ToDo.Core.SharedKernel;

namespace ToDo.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext DbContext;

        public EfRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void Add<T>(T entity) where T : IEntity
        {
            DbContext.Set<T>().Add(entity);
            DbContext.SaveChanges();
        }

        public void Add<T>(List<T> entities) where T : IEntity
        {
            DbContext.Set<T>().AddRange(entities);
            DbContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            DbContext.Set<T>().Remove(entity);
            DbContext.SaveChanges();
        }

        public void Delete<T>(IEnumerable<T> entities) where T : IEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public T GetItem<T>(Expression<Func<T, bool>> dataFilters) where T : IEntity
        {
            return DbContext.Set<T>().FirstOrDefault(dataFilters);
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters) where T : IEntity
        {
            return DbContext.Set<T>().Where(dataFilters).AsQueryable();
        }

        public IQueryable<T> GetItems<T>() where T : IEntity
        {
            return DbContext.Set<T>().AsQueryable();
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, T data) where T : IEntity
        {
            DbContext.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            DbContext.SaveChanges();
        }
    }
}
