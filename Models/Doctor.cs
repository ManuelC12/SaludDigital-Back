using System;
using System.ComponentModel.DataAnnotations;

namespace SaludDigital.Models
{
    public class Doctor
    {
        [Key]
        public Guid iDoctor { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Specialty { get; set; } = "Psicología"; // Ejemplo: Psicología o Psiquiatría

        [Required]
        [MaxLength(200)]
        public string Institution { get; set; } = string.Empty; // Hospital, clínica o consultorio

        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Url]
        public string Website { get; set; } = string.Empty;

        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty; // Breve biografía o presentación
    }
}
