using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.models;
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

        public DbSet<Reservation> Reservations { get; set; } // Add your Reservation DbSet

        public DbSet<Paiment> Paiments { get; set; } // Add your Rating DbSet

        public DbSet<AbonnementPaiment> abonnementPaiments {get ; set;}

        public DbSet<Abonnements> Abonnements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
             .HasOne(s => s.Profetionnal)
             .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
             .HasMany(s => s.Reservations)
             .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
             .HasMany(s => s.Paiments)
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
            .HasMany(p => p.services)
            .WithOne(c => c.Professional)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Profetionnal>()
            .HasMany(p => p.availabilities)
            .WithOne(c => c.profetionnal)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Profetionnal>()
                .HasMany(p => p.abonnementPaiments)
                .WithOne(a=>a.Profetionnal)
                .OnDelete(DeleteBehavior.SetNull);








            modelBuilder.Entity<Service>()
             .HasOne(s => s.Professional)
             .WithMany(p => p.services)
             .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

            


            modelBuilder.Entity<Service>()
            .HasMany(s => s.Reservations)
             .WithOne(p => p.Service)
            .OnDelete(DeleteBehavior.Cascade);







            modelBuilder.Entity<Availability>()
             .HasOne(s => s.profetionnal)
             .WithMany(p => p.availabilities)
             .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
             .HasIndex(a => new { a.DayOfWeek, a.ProfessionalId })
            .IsUnique();







            modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Service)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.ServiceId)
              .OnDelete(DeleteBehavior.Restrict); // ou .NoAction

             modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(p => p.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); // OK

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Paiment)
                .WithOne(s => s.Reservation)
                .OnDelete(DeleteBehavior.Cascade); // OK






            modelBuilder.Entity<Paiment>()
                .HasOne(p => p.Reservation)
                .WithOne(r => r.Paiment)
                .HasForeignKey<Paiment>(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // OK

            modelBuilder.Entity<Paiment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Paiments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // OK


            modelBuilder.Entity<AbonnementPaiment>()
                .HasOne(a=> a.Profetionnal)
                .WithMany(p => p.abonnementPaiments)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AbonnementPaiment>()
                .HasOne(a => a.Abonnement)
                .WithMany(p => p.abonnementPaiments)
                .HasForeignKey(foreignKeyExpression => foreignKeyExpression.AbonnementId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Abonnements>()
                .HasMany(a => a.abonnementPaiments)
                .WithOne(p => p.Abonnement)
                .OnDelete(DeleteBehavior.SetNull);


        }

      

         
        


    }
}
