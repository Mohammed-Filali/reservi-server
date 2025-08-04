using server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.DTOS
{
    public class ReservationDTOS
    {

        [Required]
        public DateOnly Date { get; set; }

        [Required]

        public TimeOnly Time { get; set; }



        public string User { get; set; } = null!;  // Assuming ApplicationUser.Id is string (Identity default)

        public int Service { get; set; }   // Assuming ApplicationUser.Id is string (Identity default)
    }
}
