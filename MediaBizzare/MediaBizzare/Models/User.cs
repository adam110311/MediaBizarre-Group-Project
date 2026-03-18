namespace MediaBizzare.Models
{
    public class User
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string bank_account { get; set; }
        public string password_hash { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
