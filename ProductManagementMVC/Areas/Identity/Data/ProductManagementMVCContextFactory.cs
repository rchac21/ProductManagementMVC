using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductManagementMVC.Data
{
    public class ProductManagementMVCContextFactory : IDesignTimeDbContextFactory<ProductManagementMVCContext>
    {
        public ProductManagementMVCContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductManagementMVCContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ProductManagementMVC;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new ProductManagementMVCContext(optionsBuilder.Options);
        }
    }
}
