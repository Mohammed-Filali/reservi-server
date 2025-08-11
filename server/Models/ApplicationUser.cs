using Microsoft.AspNetCore.Identity;
using server.models;

namespace server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Profetionnal? Profetionnal { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }

       public ICollection<Paiment>? Paiments { get; set; }
      



    }
}
