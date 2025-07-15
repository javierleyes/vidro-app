namespace vidro.api.Domain
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }

        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}
