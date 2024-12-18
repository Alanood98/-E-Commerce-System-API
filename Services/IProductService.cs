using E_CommerceSystem.Models;
using System.Security.Claims;

public interface IProductService
{
    void AddProduct(Product product, ClaimsPrincipal user);
    void UpdateProduct(Product product);
    Product GetProductById(int id);
    IEnumerable<Product> GetProducts(string name, decimal? minPrice, decimal? maxPrice, int page, int pageSize);

}