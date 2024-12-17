using E_CommerceSystem.Models;

public interface IProductService
{
    List<Product> GetAllProducts(int pageNumber, int pageSize);
   
    Product AddProduct(Product product);
    Product UpdateProduct(Product product);
    
}