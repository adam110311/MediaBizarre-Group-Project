using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class EmployeeRole
    {
        public Employee? Employee { get; set; }
        public Role? Role { get; set; }
    }
}
