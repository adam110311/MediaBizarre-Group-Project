using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Category
    {
        public int Id { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;
    }
}