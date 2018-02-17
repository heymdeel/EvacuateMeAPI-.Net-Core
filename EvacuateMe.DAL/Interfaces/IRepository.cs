using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvacuateMe.DAL
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> GetQueryable();
        Task CreateAsync(T item);
        Task<T> FindByIdAsync(int id);
        Task RemoveAsync(T item);
        Task UpdateAsync(T item);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> selector = null, Expression<Func<T, object>> include = null);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T,T>> selector = null, int? page = null, int? size = null, Expression<Func<T, object>> include = null);
    }
}
