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

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product GetProductById(int id) => _context.Products.Find(id);

        public IEnumerable<Product> GetAllProducts() => _context.Products.ToList();

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }


}
