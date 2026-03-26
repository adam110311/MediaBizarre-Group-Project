using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string username { get; set; } = string.Empty;

        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        public string surname { get; set; } = string.Empty;

        [Required]
        public string phone { get; set; } = string.Empty;

        [Required]
        public string email { get; set; } = string.Empty;

        [Required]
        public string bank_account { get; set; } = string.Empty;

        [Required]
        public string password_hash { get; set; } = string.Empty;

        [Required]
        public string street { get; set; } = string.Empty;

        [Required]
        public string street_number { get; set; } = string.Empty;

        [Required]
        public string postal_code { get; set; } = string.Empty;

        [Required]
        public string city { get; set; } = string.Empty;

        [Required]
        public string country { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}