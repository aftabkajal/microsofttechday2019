using EmployeeManagementSystem.Interface;
using EmployeeManagementSystem.Models.DbContexts;
using EmployeeManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EmployeeManagementSystem.Repositories
{
    public class Repository : IRepository
    {
        private readonly EmployeeDbContext DbContext;

        public Repository(EmployeeDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void Add<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Add(entity);
            DbContext.SaveChanges();
        }

        public void Add<T>(List<T> entities) where T : BaseEntity
        {
            DbContext.Set<T>().AddRange(entities);
            DbContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Remove(entity);
            DbContext.SaveChanges();
        }

        public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public T GetItem<T>(Expression<Func<T, bool>> dataFilters) where T : BaseEntity
        {
            return DbContext.Set<T>().FirstOrDefault(dataFilters);
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters) where T : BaseEntity
        {
            return DbContext.Set<T>().Where(dataFilters).AsQueryable();
        }

        public IQueryable<T> GetItems<T>() where T : BaseEntity
        {
            return DbContext.Set<T>().AsQueryable();
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, T data) where T : BaseEntity
        {
            DbContext.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            DbContext.SaveChanges();
        }
    }
}
