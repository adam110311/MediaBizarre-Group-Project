using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        public Employee? Manager { get; set; }
        public Category? Category { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
