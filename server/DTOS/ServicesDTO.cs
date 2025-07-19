using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.DTOS
{
    public class ServicesDTO
    {
        public int Professional { get; set; }

        [Required]
        [MaxLength(255)]
        public string TitleService { get; set; } = string.Empty;

        public string DescriptionService { get; set; } = string.Empty;

        public int DurationService { get; set; } // in minutes or another unit

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

    }
}
