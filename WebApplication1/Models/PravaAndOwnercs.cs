namespace WebApplication1.Models
{
    public class PravaAndOwnercs
    {
        public Guid id { get; set; }

        public DateTime date { get; set; }

        public string series { get; set; }

        public int number { get; set; }

        public DateTime date_end { get; set; }

        public string kod_podrazdeleniya { get; set; }

        public string[] type {  get; set; }

        public Person person { get; set; }

        public PravaAndOwnercs(Guid id, DateTime date, string series, int number, DateTime date_end, string kod_podrazdeleniya, string[] type, Person person)
        {
            this.id = id;
            this.date = date;
            this.series = series;
            this.number = number;
            this.date_end = date_end;
            this.kod_podrazdeleniya = kod_podrazdeleniya;
            this.type = type;
            this.person = person;
        }

        public PravaAndOwnercs() { }
    }
}
