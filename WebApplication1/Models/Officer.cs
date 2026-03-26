namespace WebApplication1.Models
{
    public class Officer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public int RankId { get; set; }

        public DateOnly BirthDate { get; set; }

        public int PassportNumber { get; set; }

        public int PassportSeries { get; set; }

        public int PoliceStationId { get; set; }
    }
}
