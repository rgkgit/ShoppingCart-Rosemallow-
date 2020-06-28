using System;

namespace ShoppingCart.Provider.Repository
{
    public class UnitOfWork
    {
        private ShoppingCartDbContext _dbContext = new ShoppingCartDbContext();
        public Type TheType { get; set; }
        public GenericRepository<T> GetRepoInstance<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
