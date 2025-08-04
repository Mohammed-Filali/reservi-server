using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class AbonnementPaiment
    {

        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public AbonnementPaymentStatus Status { get; set; } = AbonnementPaymentStatus.Pending; // Default status

        [ForeignKey("ProfetionnalId")]
        public string ProfetionnalId { get; set; } // Foreign key to Professional
        // Navigation property
        public Profetionnal? Profetionnal { get; set; }

        // Navigation property
        public string? TransactionId { get; set; } // Optional transaction ID for payment gateway integration
        public string? Currency { get; set; } = "MAD"; // Default currency

        public DateOnly updated_at { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly created_at { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
    public enum AbonnementPaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}

