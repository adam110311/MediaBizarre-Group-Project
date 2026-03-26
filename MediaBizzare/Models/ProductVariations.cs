namespace MediaBizzare.Models
{
    public class ProductVariations
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
