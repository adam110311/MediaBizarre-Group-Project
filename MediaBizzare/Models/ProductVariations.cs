using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class ProductVariations
    {
        public int Id { get; set; }
        [Required]
        public Product? Product { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
