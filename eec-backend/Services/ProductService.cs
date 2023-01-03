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
            try
            {
                var categoriesFromDb = await _context.Set<Product>()
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
                return ToFormattedCategories(categoriesFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving categories.");
                throw;
            }

            List<string> ToFormattedCategories(List<string> categoriesFromDb)
            {
                Dictionary<string, string> categoryDictionary = new Dictionary<string, string>
                {
                    ["refrigeratingappliances2019"] = "Refrigerators",
                    ["airconditioners"] = "Air Conditioners",
                    ["washingmachines2019"] = "Washing Machines",
                    ["ovens"] = "Ovens",
                    ["dishwashers2019"] = "Dish Washers"
                };

                List<string> formattedCategories = new();
                Array.ForEach(categoriesFromDb.ToArray(), (category) => formattedCategories.Add(categoryDictionary.TryGetValue(category, out var name) ? name : category));

                return formattedCategories;
            }
        }

        public async Task<IEnumerable<string>> GetSuppliersForCategory(string category)
        {
            try
            {
                var categories = await _context.Set<Product>()
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
                if (!categories.Contains(category))
                {
                    throw new CategoryNotFoundException($"Category {category} does not exist.");
                }

                var suppliers = await _context.Set<Product>()
                    .Where(p => p.Category == category)
                    .Select(p => p.SupplierOrTrademark)
                    .Distinct()
                    .ToListAsync();
                return suppliers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving suppliers.");
                throw;
            }
        }

        public class CategoryNotFoundException : Exception
        {
            public CategoryNotFoundException(string message) : base(message)
            { }
        }


        public async Task<IEnumerable<string>> GetModelIdentifiersForSupplierInCategory(string category, string supplier)
        {
            try
            {
                var categories = await _context.Set<Product>()
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
                if (!categories.Contains(category))
                {
                    throw new CategoryNotFoundException($"Category {category} does not exist.");
                }

                var suppliers = await _context.Set<Product>()
                    .Where(p => p.Category == category)
                    .Select(p => p.SupplierOrTrademark)
                    .Distinct()
                    .ToListAsync();

                if (!suppliers.Contains(supplier))
                {
                    throw new SupplierNotFoundException($"Supplier {supplier} does not exist for category {category}.");
                }

                var modelIdentifiers = await _context.Set<Product>()
                    .Where(p => p.Category == category && p.SupplierOrTrademark == supplier)
                    .Select(p => p.ModelIdentifier)
                    .ToListAsync();
                return modelIdentifiers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving model identifiers.");
                throw;
            }
        }

        public class SupplierNotFoundException : Exception
        {
            public SupplierNotFoundException(string message) : base(message)
            { }
        }


        private bool ProductExists(string modelIdentifier)
        {
            return _context.Products.Any(p => p.ModelIdentifier == modelIdentifier);
        }
    }
}
