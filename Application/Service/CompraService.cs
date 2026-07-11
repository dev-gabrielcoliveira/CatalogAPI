using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Events;
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
            var jogo = _jogoService.ObterPorId(input.IdJogo);

            if (jogo == null)
                throw new KeyNotFoundException("Jogo não encontrado.");

            var orderPlacedEvent = new OrderPlacedEvent
            {
                UserId = input.IdUsuario,
                GameId = jogo.Id,
                Price = jogo.Preco
            };

            await _publishEndpoint.Publish(orderPlacedEvent);

            _logger.LogInformation(
                "[CatalogAPI] Pedido publicado. Usuário: {UserId}, Jogo: {GameId}, Preço: {Price}",
                input.IdUsuario,
                jogo.Id,
                jogo.Preco);
        }

    }
}
