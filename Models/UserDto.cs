    using System;
    using System.ComponentModel.DataAnnotations;

    namespace SaludDigital.Models
    {
        public class UserDto
        {
            [Key]
            public Guid iUser { get; set; } = Guid.NewGuid();

            [Required]
            [MaxLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            [MaxLength(20)]
            public string PhoneNumber { get; set; } = string.Empty;

            [Required]
            [MinLength(6)]
            public string Password { get; set; } = string.Empty;

            [Range(0, 120)]
            public int Age { get; set; }

            [Required]
            [MaxLength(1)]
            public string Gender { get; set; } = string.Empty; // Ejemplo: "M" ó "F"

            [Required]
            [MinLength(1)]
            public string isDoctor { get; set; } = "N";
        }
    }
