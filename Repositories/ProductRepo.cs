using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

using Microsoft.Identity.Client;


using E_CommerceSystem;



namespace E_CommerceSystem.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts(int pageNumber, int pageSize)
        {
            try
            {
                return _context.Products
                    
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error retrieving Product: {ex.Message}");
                throw;
            }

        }

       

        public Product AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return product;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding new Product: {ex.Message}");
                throw; // Rethrow the exception if necessary
            }

        }
        public Product UpdateProduct(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Product with ID {product.ProductId}: {ex.Message}");
                throw;
            }
        }
        

    }
}