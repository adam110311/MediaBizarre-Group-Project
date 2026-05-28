using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class ProductVariations
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [StringLength(100)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        // Optional image URL for this specific variation (e.g. colour variant photo).
        // Falls back to the first variation that has an image when displaying a product card.
        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}