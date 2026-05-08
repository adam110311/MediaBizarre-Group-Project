using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<ProductVariations> Variations { get; set; } = new List<ProductVariations>();
    }
}
