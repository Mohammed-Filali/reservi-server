using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Profetionnal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;  // Assuming ApplicationUser.Id is string (Identity default)

        [Required]
        public string BusinessName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        public string? ProfileImage { get; set; }

        [Required]
        [EnumDataType(typeof(ProfetionnalStatus))]
        public ProfetionnalStatus Status { get; set; } = ProfetionnalStatus.Active;

        public string? Phone { get; set; }

        // Navigation property to the user
        public ApplicationUser User { get; set; } = null!;

        public int? CategoryId { get; set; }

        // Navigation property
        public Categorie? Category { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Availability>? Availabilities { get; set; }

    }

    public enum ProfetionnalStatus
    {
        Pending,
        Active,
        Blocked
    }


}

