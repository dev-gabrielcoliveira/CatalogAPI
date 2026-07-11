namespace FCG.CatalogAPI.Application.Events
{
    public record OrderPlacedEvent
    {
        public int UserId { get; init; }
        public int GameId { get; init; }
        public decimal Price { get; init; }
    }
}
