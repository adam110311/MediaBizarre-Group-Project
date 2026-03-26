namespace MediaBizzare.Models
{
    public class CategoryProduct
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        // Navigation properties
        public Category Category { get; set; }
        public Product Product { get; set; }
    }
}
