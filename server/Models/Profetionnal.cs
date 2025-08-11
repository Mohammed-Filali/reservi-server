using server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.models
{
    public class Profetionnal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string BusinessName { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public string? ProfileImage { get; set; }

        [Required]
        public ProfetionnalStatus Status { get; set; } = ProfetionnalStatus.Pending; // Default status

        public string? Phone { get; set; }

        public int? CategoryId { get; set; }

        // Relations (optionnel, à adapter selon vos besoins)
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("CategoryId")]
        public Categorie? Category { get; set; }

        public ICollection<Service>? services { get; set; }   
        
        public ICollection<Availability>? availabilities { get; set; }

        public ICollection<AbonnementPaiment>?  abonnementPaiments { get; set; }


    }

    public enum ProfetionnalStatus
    {
        Pending ,
        Active  ,
        Rejected ,
    }
}
