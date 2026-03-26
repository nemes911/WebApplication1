namespace WebApplication1.Models
{
    public class District
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    public class DistrictCount
    {
        public int DistrictId { get; set; }

        public int? IncidentCount { get; set; }

        public decimal? TotalDamage { get; set; }

        public DistrictCount(int DistrictId, int? IncidentCount, decimal? TotalDamage)
        {
            this.DistrictId = DistrictId;
            this.IncidentCount = IncidentCount;
            this.TotalDamage = TotalDamage;
        }
        public DistrictCount() { }
    }
}
