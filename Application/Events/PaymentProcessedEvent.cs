namespace FCG.Application.Events
{
    public record PaymentProcessedEvent
    {
        public int UserId { get; init; }
        public int GameId { get; init; }
        public decimal Price { get; init; }
        public string Status { get; init; }
    }
}
