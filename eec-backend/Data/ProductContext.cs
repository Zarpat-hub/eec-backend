using eec_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace eec_backend.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string connectionString = @"Server=sql.bsite.net\MSSQL2016;Initial Catalog=zarpatu_eec; User ID=zarpatu_eec; Password=eecteam123;Trust Server Certificate=true";
            options.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        }
    }
}
