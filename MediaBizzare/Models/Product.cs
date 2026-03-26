using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public Category? Category { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
