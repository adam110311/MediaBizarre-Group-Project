using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public User? User { get; set; }

        [Required]
        public Department? Department { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(30)]
        public string EmergencyPhone { get; set; } = string.Empty;
    }
}