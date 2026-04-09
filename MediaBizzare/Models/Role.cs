using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
    }
}