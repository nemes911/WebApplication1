using System;

namespace WebApplication1.Models
{
    public class JoinPrava
    {
        public Prava prava { get; set; }

        public Person Person { get; set; }

        public Vehicle Vehicle { get; set; }

        public JoinPrava(Prava prava, Person person, Vehicle vehicle)
        {
            this.prava = prava;
            Person = person;
            Vehicle = vehicle;
        }
        public JoinPrava() { }
    }
}
