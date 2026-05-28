using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Street { get; set; }

        [StringLength(20)]
        public string? StreetNumber { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
    }

    public class EditEmployeeViewModel
    {
        public int EmployeeId { get; set; }

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
