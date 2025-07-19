using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.DTOS
{
    public class AvaiblitiesDTO
    {
        [Required]
        public string Day { get; set; } = string.Empty;

        [Required]
        public TimeSpan Start { get; set; }

        [Required]
        public TimeSpan End{ get; set; }

        [ForeignKey("Professional")]
        public int ProfessionalID { get; set; }
    }
}
