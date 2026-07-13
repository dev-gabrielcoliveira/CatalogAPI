using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.CatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public ComprasController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        /// <summary>
        /// Realiza a solicitação de compra de um jogo.
        /// </summary>
        /// <param name="input">Dados da compra contendo usuário, jogo e valor.</param>
        /// <returns>
        /// Retorna 202 Accepted quando a solicitação de compra é aceita para processamento.
        /// </returns>
        /// <response code="202">Compra aceita para processamento.</response>
        /// <response code="400">Dados da compra inválidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão para realizar compras.</response>
        [HttpPost]
        [Authorize(Policy = "Usuario")]
        public async Task<IActionResult> EfetuarCompra([FromBody] CompraInput input)
        {
            await _compraService.EfetuarCompra(input);

            return Accepted();

        }
    }
}
