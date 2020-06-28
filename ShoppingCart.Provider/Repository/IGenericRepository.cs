using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShoppingCart.Provider.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetById(long id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        void Delete(T entity);
        //void DeleteRange(List<T> entities);
        void BulkDelete(Expression<Func<T, bool>> expression);
    }
}
