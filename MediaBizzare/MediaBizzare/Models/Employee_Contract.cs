namespace MediaBizzare.Models
{
    public class Employee_Contract
    {
        public int Id { get; set; }
        public int employeeId { get; set; }
        public DateTime signature_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int salary { get; set; }
        public int hours_per_week { get; set; }
        public string contract_type { get; set; }
    }
}
