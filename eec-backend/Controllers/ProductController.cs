﻿using eec_backend.Models;
using eec_backend.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
