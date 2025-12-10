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

        // 1. AGENDAR UNA CITA
        [HttpPost]
        public async Task<ActionResult<Cita>> Agendar(Cita cita)
        {
            // Generar link de reunión simulado (podrías integrar Zoom/Jitsi aquí)
            cita.LinkVideollamada = $"https://meet.jit.si/saluddigital-{Guid.NewGuid()}";
            cita.Estado = "Pendiente";

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return Ok(cita);
        }

        // 2. VER CITAS DE UN PACIENTE
        [HttpGet("paciente/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorPaciente(int id)
        {
            return await _context.Citas
                .Include(c => c.Terapeuta) // Traer datos del doctor
                .Where(c => c.PacienteId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        // 3. VER CITAS DE UN TERAPEUTA (Para el nuevo Dashboard)
        [HttpGet("terapeuta/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorTerapeuta(int id)
        {
            return await _context.Citas
                .Include(c => c.Paciente) // Traer datos del paciente
                .Where(c => c.TerapeutaId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }
    }
}