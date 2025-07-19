using Microsoft.AspNetCore.Identity;

namespace server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Profetionnal? Profetionnal { get; set; }

    }
}
