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

        // Relación con PACIENTE (Los usuarios sí usan INT, así que esto se queda igual)
        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Usuario? Paciente { get; set; }

        // --- CORRECCIÓN AQUÍ ---
        // Tu tabla Doctor usa GUID, así que aquí debe ser GUID también.
        public Guid TerapeutaId { get; set; }

        [ForeignKey("TerapeutaId")]
        public Doctor? Terapeuta { get; set; }
    }
}