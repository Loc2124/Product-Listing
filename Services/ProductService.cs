using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly ProductDbContext _context;
    public ProductService(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.OrderByDescending(p => p.CreatedAt).ToListAsync();
        
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");

        product.CreatedAt = DateTime.Now; // tự động gán thời gian hiện tại
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        // Cập nhật các trường
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.DiscountPrice = product.DiscountPrice;
        existingProduct.Stock = product.Stock;
        existingProduct.Category = product.Category;
        existingProduct.Image = product.Image;
        existingProduct.IsActive = product.IsActive;
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetPagedProductsAsync(int page, int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
            throw new ArgumentException("Page and pageSize must be greater than 0.");

        return await _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

//         return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, createdProduct);