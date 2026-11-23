using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResidentEvil.Models; // Asigură-te că namespace-ul modelelor tale este corect

namespace ResidentEvil.Data
{
    // MODIFICARE CRITICĂ: Moștenim din IdentityDbContext în loc de DbContext simplu
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabelele aplicației tale Resident Evil
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }

        // NOTĂ: Dacă aveai o logică specială în clasa ta custom 'Users', 
        // o poți păstra ca tabel secundar sau o poți șterge deoarece 
        // IdentityDbContext adaugă automat tabelul AspNetUsers gestionat de IdentityUser.
        // public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Lasă apelul de base.OnModelCreating! 
            // El configurează automat toate cheile și tabelele interne pentru Identity.
            base.OnModelCreating(modelBuilder);
        }
    }
}