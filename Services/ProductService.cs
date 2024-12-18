using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using System.Security.Claims;



namespace E_CommerceSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public void AddProduct(Product product, ClaimsPrincipal user)
        {
            var isAdmin = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admin users can add products.");
            }
            ValidateProduct(product);

            _productRepo.AddProduct(product);
        }

        // Update an existing product
        public void UpdateProduct(Product product)
        {
            ValidateProduct(product);

            var existingProduct = _productRepo.GetProductById(product.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _productRepo.UpdateProduct(product);
        }

        // Get a product by its ID
        public Product GetProductById(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            return product;
        }

        // Get products with pagination and filtering
        public IEnumerable<Product> GetProducts(string? name, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var products = _productRepo.GetAllProducts().AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrEmpty(name))
                products = products.Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));

            // Filter by minimum price if provided
            if (minPrice.HasValue)
                products = products.Where(p => p.ProductPrice >= minPrice.Value);

            // Filter by maximum price if provided
            if (maxPrice.HasValue)
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);

            // Apply pagination
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }



        // Validate product data
        private void ValidateProduct(Product product)
        {
            if (product.ProductPrice <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }

            if (product.Stock < 0)
            {
                throw new ArgumentException("Product stock cannot be negative.");
            }
        }
    }
}

