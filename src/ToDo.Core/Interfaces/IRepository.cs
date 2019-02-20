using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ToDo.Core.SharedKernel;

namespace ToDo.Core.Interfaces
{
    public interface IRepository
    {
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters) where T : IEntity;
        IQueryable<T> GetItems<T>() where T : IEntity;
        T GetItem<T>(Expression<Func<T, bool>> dataFilters) where T : IEntity;
        void Add<T>(T entity) where T : IEntity;
        void Add<T>(List<T> entities) where T : IEntity;
        void Update<T>(Expression<Func<T, bool>> dataFilters, T data) where T : IEntity;
        void Delete<T>(T entity) where T : IEntity;
        void Delete<T>(IEnumerable<T> entities) where T : IEntity;
    }
}
