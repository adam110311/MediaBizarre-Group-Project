using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Department
    {
        public int Id { get; set; }

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}