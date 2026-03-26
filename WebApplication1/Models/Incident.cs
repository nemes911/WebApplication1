namespace WebApplication1.Models
{
    public class Incident
    {
        public Guid Id { get; set; }

        public int? IncidentClassId { get; set; }

        public DateOnly IncidentDate { get; set; }

        public string Description { get; set; } = null!;

        public decimal? RepairCost { get; set; }

        public DateTime Timestamp { get; set; }

        public string Location { get; set; } = null!;

        public int PoliceStationId { get; set; }
    }
}
