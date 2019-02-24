using EmployeeManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Interface
{
    public interface IRepository
    {
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters) where T : BaseEntity;
        IQueryable<T> GetItems<T>() where T : BaseEntity;
        T GetItem<T>(Expression<Func<T, bool>> dataFilters) where T : BaseEntity;
        void Add<T>(T entity) where T : BaseEntity;
        void Add<T>(List<T> entities) where T : BaseEntity;
        void Update<T>(Expression<Func<T, bool>> dataFilters, T data) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        void Delete<T>(IEnumerable<T> entities) where T : BaseEntity;
    }
}
