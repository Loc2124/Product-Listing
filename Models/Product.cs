namespace ProductApi.Models 
{
    public class Product 
    { 
        public int ProductId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; } 
        public decimal? DiscountPrice { get; set; } 
        public int Stock { get; set; } 
        public string Category { get; set; } 
        public string Image { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public bool IsActive { get; set; } 
    } 
}

