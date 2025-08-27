namespace vidro.api.Domain;

public class Glass : Entity<Guid>
{
    public int Order { get; set; }

    public string Name { get; set; }

    public decimal? PriceTransparent { get; set; }

    public decimal? PriceColor { get; set; }

    public DateTime? ModifyDate { get; set; }
}
