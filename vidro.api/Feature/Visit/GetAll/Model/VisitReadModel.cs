namespace vidro.api.Feature.Visit.GetAll.Model
{
    public class VisitReadModel
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}