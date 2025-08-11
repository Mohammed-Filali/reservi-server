using server.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [ForeignKey("Professional")]
        public int ProfessionalId { get; set; }

        public Profetionnal? profetionnal { get; set; }
    }
}
