namespace WebApplication1.Models
{
    public class IncidentOfficer
    {
        public Guid Id { get; set; }

        public Guid? IncidentId { get; set; }

        public Guid? OfficerId { get; set; }
    }
}
