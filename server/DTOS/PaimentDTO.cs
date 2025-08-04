using server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.DTOS
{
    public class PaimentDTO
    {
        
            [Key]
            public int Id { get; set; }

            public decimal total { get; set; }

            [Required]
            public string Method { get; set; } = string.Empty;
            [Required]
            public DateTime Date { get; set; }
            

            public string User { get; set; } // Foreign key to Professional
                                               // Navigation property


            public int Reservation{ get; set; } // Foreign key to Reservation

            // Navigation property

            public string? Transaction { get; set; } // Optional transaction ID for payment gateway integration
            public string? Currency { get; set; } = "MAD"; // Default currency

        
        
    }
}
