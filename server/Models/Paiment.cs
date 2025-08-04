using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Paiment
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

           [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending; // Default status

        [ForeignKey("UserId")]
        public string UserId { get; set; } // Foreign key to Professional
        // Navigation property
        public ApplicationUser? User { get; set; }

        [ForeignKey("ReservationId")]

        public int ReservationId { get; set; } // Foreign key to Reservation

        // Navigation property
        public Reservation? Reservation { get; set; }

       public string? TransactionId { get; set; } // Optional transaction ID for payment gateway integration
        public string? Currency { get; set; } = "MAD"; // Default currency

        public DateOnly updated_at { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly created_at { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}
