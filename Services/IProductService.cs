using ProductApi.Models;

namespace ProductApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<List<Product>> GetPagedProductsAsync(int page, int pageSize); // Thêm phương thức hỗ trợ phân trang
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
}