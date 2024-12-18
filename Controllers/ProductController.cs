using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.UserDTO;
using System.Security.Claims;
using E_CommerceSystem.Utils;

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

        [HttpPost("Add Product")]
        
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct([FromBody] ProductInput productInput)
        {
            try
            {
                // Extract token from Authorization header
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Authorization token is missing.");
                }

                // Extract role from the token
                var role = JwtHelper.GetUserRoleFromToken(token);
                if (role != "Admin")
                {
                    return Forbid(); // Use Forbid() without a custom string
                }

                // Map input to Product entity
                var product = new Product
                {
                    ProductName = productInput.ProductName,
                    ProductDescription = productInput.ProductDescription,
                    ProductPrice = productInput.ProductPrice,
                    Stock = productInput.Stock
                };

                _productService.AddProduct(product,User);
                return Ok(new { Message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

       [HttpPut("{id}")]
[Authorize(Roles = "Admin")]
public IActionResult UpdateProduct(int id, [FromBody] ProductInput productInput)
{
    try
    {
        // Fetch the existing product from the database
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null)
        {
            return NotFound(new { Error = $"Product with ID {id} not found." });
        }

        // Update the properties of the existing product
        existingProduct.ProductName = productInput.ProductName;
        existingProduct.ProductDescription = productInput.ProductDescription;
        existingProduct.ProductPrice = productInput.ProductPrice;
        existingProduct.Stock = productInput.Stock;

        // Save changes using the service
        _productService.UpdateProduct(existingProduct);

        return Ok(new { Message = "Product updated successfully." });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { Error = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { Error = ex.Message });
    }
}


        // Get a product by ID (Public)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Get products with filtering and pagination (Public)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProducts(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var products = _productService.GetProducts(name, minPrice, maxPrice, page, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
