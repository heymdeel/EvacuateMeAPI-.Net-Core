using EvacuateMe.DAL.Contexts;
using EvacuateMe.DAL.Entities;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace EvacuateMe.DAL
{
    public class L2DBRepository<T> : IRepository<T> where T : Entity
    {
        public async Task CreateAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.InsertAsync(item);
            }
        }

        public async Task<T> FindByIdAsync(int id)
        {
            using (var db = new L2DBContext())
            {
                return await db.GetTable<T>().Where(e => e.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, System.Linq.Expressions.Expression<Func<T, T>> selector = null, System.Linq.Expressions.Expression<Func<T, object>> include = null)
        {
            using (var db = new L2DBContext())
            {
                IQueryable<T> query;

                if (include != null)
                {
                    query = db.GetTable<T>().LoadWith(include);
                }
                else
                {
                    query = db.GetTable<T>();
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (selector != null)
                {
                    query = query.Select(selector);
                }

                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> selector = null, int? page = null, int? size = null, Expression<Func<T, object>> include = null)
        {
            using (var db = new L2DBContext())
            {
                IQueryable<T> query;

                if (include != null)
                {
                    query = db.GetTable<T>().LoadWith(include);
                }
                else
                {
                    query = db.GetTable<T>();
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
                    query = query.Take((page.Value - 1) * size.Value).Skip(size.Value);
                }

                return await query.ToListAsync();
            }
        }

        public IQueryable<T> GetQueryable()
        {
            var db = new L2DBContext();

            return db.GetTable<T>().AsQueryable();
        }

        public async Task RemoveAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.DeleteAsync(item);
            }
        }

        public async Task UpdateAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.UpdateAsync(item);
            }
        }
    }
}
