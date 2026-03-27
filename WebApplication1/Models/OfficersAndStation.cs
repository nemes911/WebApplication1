namespace WebApplication1.Models
{
    public class OfficersAndStation
    {
        public Guid id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int RankId { get; set; }

        public DateOnly BirthDate { get; set; }

        public int passport_number { get; set; }

        public int passportSeries { get; set; }

        public int police_station_id { get; set; }

        public int id_station { get; set; }

        public int districtId { get; set; }

        public string address { get; set; }

        public string Phone { get; set; }

        public OfficersAndStation(Guid id, string firstName, string lastName, string middleName, int rankId, DateOnly birthDate, int passport_number, int passportSeries, int police_station_id, int id_station, int districtId, string address, string phone)
        {
            this.id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            RankId = rankId;
            BirthDate = birthDate;
            this.passport_number = passport_number;
            this.passportSeries = passportSeries;
            this.police_station_id = police_station_id;
            this.id_station = id_station;
            this.districtId = districtId;
            this.address = address;
            Phone = phone;
        }

        public OfficersAndStation() { }
    }
}
