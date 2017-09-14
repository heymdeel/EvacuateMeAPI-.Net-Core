using EvacuateMe.DAL.Contexts;
using EvacuateMe.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.DAL
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        private EFPostgreSQLContext _context;
        private DbSet<T> _dbSet;

        public EFRepository(EFPostgreSQLContext db)
        {
            _context = db;
            _dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task RemoveAsync(T item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> selector = null, int? page = null, int? size = null, Expression<Func<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            if (page != null && size != null)
            {
                query = query.Skip((page.Value - 1) * size.Value).Take(size.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> selector = null, Expression<Func<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            if (include != null)
            {
                query = query.Include(include);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }
            
            return await query.FirstOrDefaultAsync();
        }
    }
}
