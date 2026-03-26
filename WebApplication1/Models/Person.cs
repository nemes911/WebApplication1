namespace WebApplication1.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public int PassportNumber { get; set; }

        public int PassportSeries { get; set; }

        public int SocialStatusId { get; set; }

        public Guid id_prav { get; set; }
    }
}
