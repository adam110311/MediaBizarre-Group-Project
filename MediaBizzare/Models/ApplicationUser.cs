using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MediaBizzare.Models
{
    // Inherits IdentityUser<int> so we keep integer primary keys (matches existing FKs like Employee.UserId).
    // IdentityUser already provides: UserName, NormalizedUserName, Email, NormalizedEmail,
    // PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, EmailConfirmed,
    // PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount.
    //
    // We REUSE Identity's columns where they overlap with the old User model:
    //   old `username`     -> IdentityUser.UserName
    //   old `email`        -> IdentityUser.Email
    //   old `phone`        -> IdentityUser.PhoneNumber
    //   old `password_hash`-> IdentityUser.PasswordHash
    //
    // The remaining domain fields (name, address, bank account) live here.
    // Address fields are made optional so registration is reasonable; users can fill them in later.
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [StringLength(50)]
        public string? BankAccount { get; set; }

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

        // Preserved from the old User model: every ApplicationUser may optionally be an Employee.
        public Employee? Employee { get; set; }
    }
}
