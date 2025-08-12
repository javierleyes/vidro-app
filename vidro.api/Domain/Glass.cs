namespace vidro.api.Domain;

public class Glass : Entity<Guid>
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public DateTime? ModifyDate { get; set; }
}
