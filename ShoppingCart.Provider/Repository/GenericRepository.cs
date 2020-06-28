using Data.EF.Core.Extensions;
using Data.EF.Core.Repositories;
using ShoppingCart.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShoppingCart.Provider.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        DbSet<T> _dbSet;
        private ShoppingCartDbContext _dbContext;
        private EfRepository<ShoppingCartDbContext, T> _repo;
        public GenericRepository(ShoppingCartDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _repo = new EfRepository<ShoppingCartDbContext, T>(_dbContext);
        }
        public async Task<bool> Add(T entity)
        {
            return await _repo.AddAsync(entity);
        }

        public async Task<bool> AddRange(List<T> entity)
        {
            return await _repo.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void BulkDelete(Expression<Func<T, bool>> expression)
        {
            try
            {
                if (expression == null)
                {
                    throw new ArgumentNullException(nameof(expression));
                }

                using (var tx = TransactionExtensions.CreateTransactionScope())
                {
                    _dbSet.Where(expression)
                        .DeleteFromQuery();
                    tx.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<T> GetAll()
        {
            return _repo.GetAllAsNoTracking();
        }

        public async Task<T> GetById(long id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            return await _repo.AddOrUpdateAsync(entity);
        }
    }
}
