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

        // AGENDAR CITA
        [HttpPost]
        public async Task<ActionResult<Cita>> Agendar(Cita cita)
        {
            // Generar link único
            cita.LinkVideollamada = $"https://meet.jit.si/saluddigital-{Guid.NewGuid()}";
            cita.Estado = "Pendiente";

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return Ok(cita);
        }

        // VER CITAS DE UN PACIENTE
        [HttpGet("paciente/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorPaciente(int id)
        {
            return await _context.Citas
                .Include(c => c.Terapeuta) // Coincide con Models/Cita.cs
                .Where(c => c.PacienteId == id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        // VER CITAS DE UN TERAPEUTA
        [HttpGet("doctor/{id}")]
        public async Task<ActionResult<List<Cita>>> GetPorTerapeuta(int id)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Where(c => c.TerapeutaId == id) // Coincide con Models/Cita.cs
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }
    }
}