using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaludDigital.Models
{
    public class EmotionRecord
    {
        [Key]
        public Guid iRecord { get; set; } = Guid.NewGuid();

        // Relación con el usuario que registró la emoción
        [Required]
        public Guid iUser { get; set; }

        [ForeignKey("iUser")]
        public Usuario? User { get; set; }

        // Sentimiento o emoción seleccionada
        [Required]
        [MaxLength(50)]
        public string Emotion { get; set; } = string.Empty;
        /*
            Ejemplos posibles:
            - Feliz
            - Triste
            - Ansioso
            - Enojado
            - Estresado
            - Relajado
            - Motivado
            - Deprimido
        */

        // Fecha y hora en que el usuario registró su emoción
        [Required]
        public DateTime RecordedAt { get; set; } = DateTime.Now;

        // (Opcional) Comentario o nota para contexto
        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
