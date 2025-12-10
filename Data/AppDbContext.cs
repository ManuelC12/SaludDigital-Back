using Microsoft.EntityFrameworkCore;
using SaludDigital.Models;

namespace SaludDigital.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Users { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<EmotionRecord> EmotionRecords { get; set; } = null!; // Si lo tenías

        // --- ESTA LÍNEA ES LA CLAVE PARA ARREGLAR EL ERROR DE BUILD ---
        public DbSet<Cita> Citas { get; set; } = null!;
    }
}