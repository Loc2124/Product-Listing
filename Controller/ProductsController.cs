using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Services;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService, ProductDbContext dbContext) : ControllerBase
{
    private readonly IProductService _productService = productService;
     private readonly List<Product> _products = new List<Product>();
    private readonly ProductDbContext _dbContext = dbContext;

    // GET: /api/products
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 9)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be greater than 0.");
        }

        // Lấy danh sách sản phẩm phân trang từ tầng dịch vụ
        var pagedProducts = await _productService.GetPagedProductsAsync(page, pageSize);
        var totalProducts = await _dbContext.Products.CountAsync(); // Tổng số sản phẩm

        // Trả về dữ liệu cùng với thông tin phân trang
        var response = new
        {
            TotalItems = totalProducts,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize),
            Data = pagedProducts
        };

        return Ok(response);
    }

    // GET: /api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid product ID.");

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST: /api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        try
        {
            if (product == null)
                return BadRequest("Product data is required.");

            if (string.IsNullOrEmpty(product.Name))
                return BadRequest("Product name is required.");

            if (product.Price <= 0)
                return BadRequest("Product price must be greater than 0.");

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, createdProduct);
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: /api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (product == null)
            return BadRequest("Product data is required.");

        if (string.IsNullOrEmpty(product.Name))
            return BadRequest("Product name is required.");

        if (product.Price <= 0)
            return BadRequest("Product price must be greater than 0.");

        var updatedProduct = await _productService.UpdateProductAsync(id, product);
        if (updatedProduct == null)
            return NotFound();

        return Ok(updatedProduct);
    }

    // DELETE: /api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}