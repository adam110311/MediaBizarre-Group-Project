using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Employee_Contract
    {
        public int Id { get; set; }
        [Required]
        public Employee? Employee { get; set; }
        [Required]
        public DateTime signature_date { get; set; }
        [Required]
        public DateTime start_date { get; set; }
        [Required]
        public DateTime end_date { get; set; }
        [Required]
        public int salary { get; set; }
        [Required]
        public int hours_per_week { get; set; }
        [Required]
        public string contract_type { get; set; }
    }
}
