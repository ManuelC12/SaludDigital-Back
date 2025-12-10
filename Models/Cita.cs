using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaludDigital.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string LinkVideollamada { get; set; } = "";

        // Relación con PACIENTE
        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Usuario? Paciente { get; set; }

        // Relación con TERAPEUTA (Usando la clase Doctor)
        public int TerapeutaId { get; set; }
        [ForeignKey("TerapeutaId")]
        public Doctor? Terapeuta { get; set; }
    }
}