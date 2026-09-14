using FCG.CatalogAPI.Application.DTOs;
using FCG.Application.Events;
using FCG.CatalogAPI.Application.Interfaces.Service;
using MassTransit;

namespace FCG.CatalogAPI.Application.Service
{
    public class CompraService : ICompraService
    {

        private readonly IJogoService _jogoService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CompraService> _logger;

        public CompraService(
            IJogoService jogoService,
            IPublishEndpoint publishEndpoint,
            ILogger<CompraService> logger)
        {
            _jogoService = jogoService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task EfetuarCompra(CompraInput input)
        {
            if (input.IdUsuario <= 0)
                throw new ArgumentException("Usuário inválido.");

            var jogo = await _jogoService.ObterPorIdAsync(input.IdJogo);

            if (jogo == null)
                throw new ArgumentException("Jogo não foi encontrado.");

            var orderPlacedEvent = new OrderPlacedEvent
            {
                UserId = input.IdUsuario,
                GameId = jogo.IdJogo,
                Price = jogo.Preco
            };

            await _publishEndpoint.Publish(orderPlacedEvent);

            _logger.LogInformation(
                "[CatalogAPI] Pedido publicado. Usuário: {UserId}, Jogo: {GameId}, Preço: {Price}",
                input.IdUsuario,
                jogo.IdJogo,
                jogo.Preco);
        }

    }
}
