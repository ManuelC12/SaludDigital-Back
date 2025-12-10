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

        // AGENDAR
        [HttpPost]
        public async Task<ActionResult<Cita>> Agendar(Cita cita)
        {
            cita.LinkVideollamada = $"https://meet.jit.si/saluddigital-{Guid.NewGuid()}";
            cita.Estado = "Pendiente";

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return Ok(cita);
        }

        // VER CITAS PACIENTE (Ahora recibe Guid)
        [HttpGet("paciente/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorPaciente(Guid id)
        {
            return await _context.Citas
                .Include(c => c.Terapeuta)
                .Where(c => c.PacienteId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        // VER CITAS TERAPEUTA (Ahora recibe Guid)
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