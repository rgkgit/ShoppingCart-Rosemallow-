using Data.EF.Core.DbContexts;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ShoppingCart.Provider.EntityModel;

namespace ShoppingCart.Provider
{
    public class ShoppingCartDbContext : DbContextBase
    {
        public ShoppingCartDbContext()
            : base("ShoppingCartDbConnection")
        {
            this.SetCommandTimeOut(180);
            Database.SetInitializer<ShoppingCartDbContext>(null);
        }
        public void SetCommandTimeOut(int Timeout)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = Timeout;
        }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<AuditDetail> AuditDetails { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
