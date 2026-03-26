using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

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