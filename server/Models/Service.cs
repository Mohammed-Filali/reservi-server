using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Professional")]
        public int ProfessionalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Duration { get; set; } // in minutes or another unit

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

      
        public Profetionnal Professional { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }

    }
}
