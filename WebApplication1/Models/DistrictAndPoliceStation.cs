namespace WebApplication1.Models
{
    public class DistrictAndPoliceStation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int idPolice {  get; set; }

        public int idstrict { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public DistrictAndPoliceStation(int id, string name, int idPolice, int idstrict, string address, string phone)
        {
            Id = id;
            Name = name;
            this.idPolice = idPolice;
            this.idstrict = idstrict;
            this.address = address;
            this.phone = phone;
        }

        public DistrictAndPoliceStation() { }
    }
}
