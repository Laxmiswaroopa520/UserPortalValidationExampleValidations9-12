using System;
using System.ComponentModel.DataAnnotations;
using UserPortalValdiationsDBContext.CustomValidations;
using UserPortalValdiationsDBContext.Enums;

namespace UserPortalValdiationsDBContext.Models
{
    public class User
    {
        public int Id { get; set; }

        // USERNAME
        [Required]
        [NotAdminUsername(ErrorMessage = "Admin username not allowed.")]
        public string? Username { get; set; }

        // EMAIL
        [Required]
        [ValidEmail]
        public string? Email { get; set; }

        // PASSWORD
        [Required]
        [StrongPassword]
        [NoSequentialDigits]
        public string? Password { get; set; }

        // AGE
        [Range(1, 100)]
        [MinAge(18)]
        public int Age { get; set; }

        // PHONE
        [PhoneNumber]
        [NoSequentialDigits]
        public string? Phone { get; set; }

        // DEPARTMENT (ENUM ✔)
        public Departments? Department { get; set; }

        // GENDER
        [Required]
        public string? Gender { get; set; }

        // DOB
        [Required]
        [PastDate]
        public DateTime? DateOfBirth { get; set; }

        // COUNTRY
        [Required]
        public string? Country { get; set; }

        // HOBBIES (comma separated)
        public string? Hobbies { get; set; }

        public bool AcceptTerms { get; set; }

        // -------------------------
        // 🔐 SECURITY / AUDIT
        // -------------------------
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }

       // public bool Is2FAVerified { get; set; }   // 🔥 FOR ROLE-SENSITIVE LOGIN

        // -------------------------
        // 📸 PROFILE
        // -------------------------
        public string? ProfilePhotoPath { get; set; }

        // -------------------------
        // 🔑 ROLE (DB-DRIVEN ✔)
        // -------------------------
        public int RoleId { get; set; }           // FK
        public Role Role { get; set; } = null!;


        public bool Is2FAVerified { get; set; }   // OTP verification status
        public string? TwoFactorCode { get; set; } // OTP
        public DateTime? TwoFactorExpiry { get; set; } // OTP expiration
        public string? LastLoginIP { get; set; }  // For Manager IP checks
    }
}
