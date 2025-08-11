namespace server.DTOS
{
    public class AbonnementDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string DurationUnit { get; set; } = "month"; // "day", "month", or "year"
        public int DurationValue { get; set; } = 1;
    }
}
