using System.Text.Json.Serialization;

namespace server.Models
{
    public class Abonnements
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string DurationUnit { get; set; } = "month"; // "day", "month", or "year"
        public int DurationValue { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]

        public ICollection<AbonnementPaiment>? abonnementPaiments { get; set; }
    }
}
