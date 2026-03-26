using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string surname { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string bank_account { get; set; }
        [Required]
        public string password_hash { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string street_number { get; set; }
        [Required]
        public string postal_code { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
    }
}
