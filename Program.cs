using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductApi.Models;
using ProductApi.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5501",
                          "http://127.0.0.1:5500") // Thay đổi địa chỉ này nếu cần
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                      });
});

// Đăng ký dịch vụ Controller
builder.Services.AddControllers();

// Cấu hình DbContext sử dụng In-Memory Database
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

// Đăng ký service nghiệp vụ (sẽ tạo ở bước sau)
builder.Services.AddScoped<IProductService, ProductService>();

// Cấu hình Swagger để tạo tài liệu API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cấu hình middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1"));
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();