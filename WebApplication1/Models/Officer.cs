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

    public class IncidentOfficerDto
    {
        public Guid IncidentId { get; set; }

        public DateOnly IncidentDate { get; set; }

        public string OfficerName { get; set; }

        public string RankName { get; set; }

        public IncidentOfficerDto(Guid incidentId, DateOnly incidentDate, string officerName, string rankName)
        {
            IncidentId = incidentId;
            IncidentDate = incidentDate;
            OfficerName = officerName;
            RankName = rankName;
        }

        public IncidentOfficerDto() { }
    }
}
