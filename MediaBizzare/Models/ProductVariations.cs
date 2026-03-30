using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class ProductVariations
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public string SKU { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
