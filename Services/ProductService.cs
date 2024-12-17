using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public List<Product> GetAllProducts(int pageNumber, int pageSize)
        {
            return _productRepo.GetAllProducts(pageNumber, pageSize);
        }

    

        public Product AddProduct(Product product)
        {
           
            return _productRepo.AddProduct(product);
        }

        public Product UpdateProduct(Product product)
        {
            var existingProduct = _productRepo.GetProductById(product.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductPrice= product.ProductPrice;

            return _productRepo.UpdateProduct(existingProduct);
        }

        
    }
}
