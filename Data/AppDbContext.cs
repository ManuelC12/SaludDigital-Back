using Microsoft.EntityFrameworkCore;
using SaludDigital.Models;

namespace SaludDigital.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<EmotionRecord> EmotionRecords { get; set; }

  


    }
}
