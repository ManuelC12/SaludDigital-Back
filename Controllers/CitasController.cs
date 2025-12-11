using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaludDigital.Data;
using SaludDigital.Models;

namespace SaludDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // AGENDAR (Ahora aceptará citas sin TerapeutaId)
        [HttpPost]
        public async Task<ActionResult<Cita>> Agendar(Cita cita)
        {
            // Generar link
            cita.LinkVideollamada = $"https://meet.jit.si/saluddigital-{Guid.NewGuid()}";
            cita.Estado = "Pendiente";

            // EF Core ahora permitirá que cita.TerapeutaId sea null
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return Ok(cita);
        }

        // NUEVO ENDPOINT: Reclamar Cita (Asignar Doctor)
        [HttpPut("reclamar/{id}")]
        public async Task<IActionResult> ReclamarCita(int id, [FromBody] Guid terapeutaId)
        {
            var cita = await _context.Citas.FindAsync(id);

            if (cita == null)
            {
                return NotFound("Cita no encontrada.");
            }

            // Opcional: Verificar si ya tiene doctor
            if (cita.TerapeutaId != null)
            {
                return BadRequest("Esta cita ya ha sido reclamada por otro doctor.");
            }

            cita.TerapeutaId = terapeutaId;
            cita.Estado = "Confirmada"; // Cambiamos el estado al asignar doctor

            await _context.SaveChangesAsync();

            return Ok(cita);
        }

        // VER CITAS DISPONIBLES (Sin doctor asignado)
        [HttpGet("disponibles")]
        public async Task<ActionResult<List<Cita>>> GetCitasDisponibles()
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Where(c => c.TerapeutaId == null) // Filtramos las que no tienen doctor
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        // ... (Mantén tus métodos GetPorPaciente y GetPorTerapeuta igual) ...
        [HttpGet("paciente/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorPaciente(Guid id)
        {
            return await _context.Citas
                .Include(c => c.Terapeuta)
                .Where(c => c.PacienteId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        [HttpGet("doctor/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorTerapeuta(Guid id)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Where(c => c.TerapeutaId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }
    }
}