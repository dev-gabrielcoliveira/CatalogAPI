using FCG.Application.Events;
using MassTransit;

namespace FCG.CatalogAPI.Application.Consumers
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {
        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(
            ILogger<PaymentProcessedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var payment = context.Message;

            if (payment.Status == "Approved")
            {
                _logger.LogInformation(
                    "[CatalogAPI] Pagamento aprovado. Liberando jogo. Usuário: {UserId}, Jogo: {GameId}",
                    payment.UserId,
                    payment.GameId);
            }
            else
            {
                _logger.LogInformation(
                    "[CatalogAPI] Pagamento rejeitado. Usuário: {UserId}, Jogo: {GameId}",
                    payment.UserId,
                    payment.GameId);
            }

            return Task.CompletedTask;
        }
    }
}