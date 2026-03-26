using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Department
    {
        public int Id { get; set; }

        // nullable because a department can exist without a manager
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}