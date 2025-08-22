namespace vidro.api.Feature.Glass.Model
{
    public class GlassReadModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? PriceTransparent { get; set; }

        public decimal? PriceColor { get; set; }
    }
}