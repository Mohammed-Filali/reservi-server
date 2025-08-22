using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace server.DTOS
{
    public class AbonenmtPaimentDTO
    {
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]

        public int ProfetionnalId { get; set; } // Foreign key to Professional

        public string? TransactionId { get; set; } // Optional transaction ID for payment gateway integration
        public string? Currency { get; set; } = "MAD"; // Default currency

        public int AbonnementId { get; set; } // Foreign key to Professional


        public PaymentDetailsDto paymentDetails { get; set; } // This can be used to store additional payment details if needed
    }
}
