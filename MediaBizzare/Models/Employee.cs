using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int DepartmentId { get; set; }
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

        public Department? ManagedDepartment { get; set; }

        public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
        public ICollection<Employee_Contract> Contracts { get; set; } = new List<Employee_Contract>();
    }
}