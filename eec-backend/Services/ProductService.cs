using eec_backend.Data;
using eec_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace eec_backend.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(string modelIdentifier);
        Task<bool> SaveProduct(Product product);
        Task<bool> UpdateProduct(string modelIdentifier, Product product);
        Task<bool> DeleteProduct(string modelIdentifier);
        Task<IEnumerable<string>> GetCategories();
        Task<IEnumerable<string>> GetSuppliersForCategory(string category);
        Task<IEnumerable<string>> GetModelIdentifiersForSupplierInCategory(string category, string supplier);
    }

    public class ProductService : IProductService
    {
        private readonly ProductContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductContext productContext, ILogger<ProductService> logger)
        {
            _context = productContext;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(string modelIdentifier)
        {
            return await _context.Products.FindAsync(modelIdentifier.Replace("%2F", "/"));
        }

        public async Task<bool> SaveProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateProduct(string modelIdentifier, Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;
        }
        
        public async Task<bool> DeleteProduct(string modelIdentifier)
        {
            if (!ProductExists(modelIdentifier))
            {
                return false;
            }

            Product product = await GetProductById(modelIdentifier);

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<string>> GetCategories()
        {
            var products = await _context.Set<Product>().ToListAsync();
            return products.Select(p => p.Category).Distinct();
        }

        public async Task<IEnumerable<string>> GetSuppliersForCategory(string category)
        {
            var products = await _context.Set<Product>().Where(p => p.Category == category).ToListAsync();
            return products.Select(p => p.SupplierOrTrademark).Distinct();
        }

        public async Task<IEnumerable<string>> GetModelIdentifiersForSupplierInCategory(string category, string supplier)
        {
            var products = await _context.Set<Product>().Where(p => p.Category == category && p.SupplierOrTrademark == supplier).ToListAsync();
            return products.Select(p => p.ModelIdentifier);
        }

        private bool ProductExists(string modelIdentifier)
        {
            return _context.Products.Any(p => p.ModelIdentifier == modelIdentifier);
        }
    }
}
