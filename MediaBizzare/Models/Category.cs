using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Slug { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}