using eec_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace eec_backend.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {
        }

        /* Connection with sql server */
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Api;Integrated Security=True;");
        //}

        public DbSet<Product> Products { get; set; } = null!;
    }
}
