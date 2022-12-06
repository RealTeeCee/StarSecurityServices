using DataAccess.Data;
using DataAccess.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.ImplementRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly StarSecurityDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(StarSecurityDbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }

        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, string? thenIncludeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, string? thenIncludeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
