using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class DB_Connect : IdentityDbContext<ApplicationUser >
    {
        public DB_Connect(DbContextOptions<DB_Connect> options) : base(options) { }

        // You can add DbSets here if you have other entities, e.g.:
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Profetionnal> Profetionnals { get; set; }  // Add your Profetionnal DbSet
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
             .HasOne(s => s.Profetionnal)
             .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Profetionnal>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            modelBuilder.Entity<Profetionnal>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profetionnal)
                .HasForeignKey<Profetionnal>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Profetionnal>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Profetionnals)
             .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Profetionnal>()
            .HasMany(p => p.Services)
            .WithOne(c => c.Professional)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Profetionnal>()
            .HasMany(p => p.Availabilities)
            .WithOne(c => c.profetionnal)
            .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Service>()
             .HasOne(s => s.Professional)
             .WithMany(p => p.Services)
             .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
             .HasOne(s => s.profetionnal)
             .WithMany(p => p.Availabilities)
             .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
             .HasIndex(a => new { a.DayOfWeek, a.ProfessionalId })
            .IsUnique();
        }

      

         
        


    }
}
