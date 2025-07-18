using vidro.api.Status;

namespace vidro.api.Domain
{
    public class Visit : Entity<int>
    {
        public DateTimeOffset Date { get; set; }

        public string Address { get; set; }  

        public string Name { get; set; }

        public string Phone { get; set; }

        public VisitStatus Status { get; set; } = VisitStatus.Pending;
    }
}
