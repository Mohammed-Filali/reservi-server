using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public  DateOnly  Date { get; set; }

        [Required]

        public TimeOnly StartTime { get; set; }

        [Required]

        public  ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;  // Assuming ApplicationUser.Id is string (Identity default)

        [ForeignKey("Service")]
        public int ServiceId { get; set; }   // Assuming ApplicationUser.Id is string (Identity default)

        public ApplicationUser User { get; set; } = null!;

        public Service Service { get; set; } = null!;

        public Paiment Paiment { get; set; } = null!; // Assuming you have a Paiment model
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
}
}