using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts(int pageNumber, int pageSize);
       
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
       

    }
}
