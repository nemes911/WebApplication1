namespace WebApplication1.Models
{
    public class PoliceStation
    {
        public int Id { get; set; }

        public int DistrictId { get; set; }

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public virtual District District { get; set; } = null!;

    }
}
