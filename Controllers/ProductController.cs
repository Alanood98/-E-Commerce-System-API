using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.UserDTO;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        public ActionResult<List<Product>> GetAllProducts(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var products = _productService.GetAllProducts(pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving products: {ex.Message}");
            }
        }

        

        [HttpPost("AddProduct")]
        public ActionResult<Product> AddProduct([FromBody] ProductInput productInput)
        {
            try
            {
                var product = new Product
                {
                    ProductName = productInput.ProductName,
                    ProductDescription = productInput.ProductDescription,
                    ProductPrice = productInput.ProductPrice,
                    Stock = productInput.Stock,
                 
                };
                var addedProduct = _productService.AddProduct(product);
                return Ok(addedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding product: {ex.Message}");
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        public ActionResult<Product> UpdateProduct(int id, [FromBody] ProductInput productInput)
        {
            try
            {
                var existingProduct = _productService.GetProductById(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                existingProduct.ProductName = productInput.ProductName;
                existingProduct.ProductDescription = productInput.ProductDescription;
                existingProduct.ProductPrice = productInput.ProductPrice;
                existingProduct.Stock = productInput.Stock;

                var updatedProduct = _productService.UpdateProduct(existingProduct);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }
        }

    }
}
