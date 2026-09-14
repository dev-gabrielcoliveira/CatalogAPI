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
        private readonly ILogger<JogosController> _logger;

        public ComprasController(ICompraService compraService, ILogger<JogosController> logger)
        {
            _compraService = compraService;
            _logger = logger;
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
            try
            {
                await _compraService.EfetuarCompra(input);

                return Accepted();
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao efetuar a compra");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }
    }
}
