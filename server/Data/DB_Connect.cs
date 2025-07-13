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
    }
}
