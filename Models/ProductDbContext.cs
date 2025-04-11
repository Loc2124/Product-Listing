using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }  // Đại diện cho bảng Products

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    { }
}