using eec_backend.Models;
using eec_backend.Services;
using Microsoft.AspNetCore.Mvc;
using static eec_backend.Services.ProductService;

namespace eec_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            List<Product> products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{modelIdentifier}")]
        public async Task<ActionResult<Product>> GetProduct(string modelIdentifier)
        {
            Product product = await _productService.GetProductById(modelIdentifier);

            if (product == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Product with given identifier does not exist in database");
            }

            return Ok(product);
        }

        [HttpPut("{modelIdentifier}")]
        public async Task<IActionResult> UpdateProduct(string modelIdentifier, Product product)
        {
            return await _productService.UpdateProduct(modelIdentifier, product)
                ? Ok()
                : StatusCode(StatusCodes.Status500InternalServerError, "An error occured, check application logs for more details");
        }

        [HttpPost]
        public async Task<ActionResult<Product>> SaveProduct(Product product)
        {
            return await _productService.SaveProduct(product)
                ? Ok()
                : StatusCode(StatusCodes.Status500InternalServerError, "An error occured, check application logs for more details");
        }

        [HttpDelete("{modelIdentifier}")]
        public async Task<IActionResult> DeleteProduct(string modelIdentifier)
        {
            return await _productService.DeleteProduct(modelIdentifier)
                 ? Ok()
                 : StatusCode(StatusCodes.Status500InternalServerError, "An error occured, check application logs for more details");
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _productService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving categories.");
            }
        }


        [HttpGet("suppliers/{category}")]
        public async Task<ActionResult<IEnumerable<string>>> GetSuppliersForCategory(string category)
        {
            try
            {
                var suppliers = await _productService.GetSuppliersForCategory(category);
                return Ok(suppliers);
            }
            catch (CategoryNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving suppliers.");
            }
        }


        [HttpGet("modelIdentifiers/{category}/{supplier}")]
        public async Task<ActionResult<IEnumerable<string>>> GetModelIdentifiersForSupplierInCategory(string category, string supplier)
        {
            try
            {
                var modelIdentifiers = await _productService.GetModelIdentifiersForSupplierInCategory(category, supplier);
                return Ok(modelIdentifiers);
            }
            catch (CategoryNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (SupplierNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving model identifiers.");
            }
        }

    }
}
