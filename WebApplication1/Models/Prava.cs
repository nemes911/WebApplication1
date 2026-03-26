namespace WebApplication1.Models
{
    public class Prava
    {
        public Guid id { get; set; }

        public DateTime date { get; set; }

        public string series { get; set; }

        public int number { get; set; }

        public DateTime date_end { get; set; }

        public string kod_podrazdeleniya { get; set; }

        public string[] type { get; set; }

        public bool status { get; set; }
    }
}
