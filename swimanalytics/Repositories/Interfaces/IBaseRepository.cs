﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace swimanalytics.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
