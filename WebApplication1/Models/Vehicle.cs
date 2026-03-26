namespace WebApplication1.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public int SerialNumber { get; set; }

        public string? Color { get; set; }

        public Guid? OwnerId { get; set; }

        public string CarBrand { get; set; }

        public string? Insurance_company { get; set; }

        public string Vin { get; set; }
    }
}
