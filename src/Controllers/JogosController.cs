using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FCG.CatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;
        private readonly ILogger<JogosController> _logger;

        public JogosController(IJogoService jogoService, ILogger<JogosController> logger)
        {
            _jogoService = jogoService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "AdministradorOuUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Jogo>>> ObterTodos()
        {
            try
            {
                var jogos = await _jogoService.ObterTodosAsync();
                return Ok(jogos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os jogos.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdministradorOuUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorId(string id)
        {
            try
            {
                var jogo = await _jogoService.ObterPorIdAsync(id);
                if (jogo == null)
                    return NotFound(new { mensagem = "Jogo não encontrado." });

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter jogo com Id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Criar([FromBody] JogoCriarInput input)
        {
            try
            {
                var jogoCriado = await _jogoService.CriarAsync(input);
                _logger.LogInformation("Jogo '{Nome}' criado com sucesso no MongoDB.", input.Nome);

                return CreatedAtAction(nameof(ObterPorId), new { id = jogoCriado.Id }, jogoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao incluir o jogo: {Nome}", input?.Nome);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(string id, [FromBody] JogoAtualizarInput inputJogo)
        {
            if (id != inputJogo.IdJogo)
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do corpo da requisição." });

            try
            {
                await _jogoService.AtualizarAsync(inputJogo);
                _logger.LogInformation("Jogo com Id {Id} foi atualizado com sucesso.", id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar jogo com Id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Remover(string id)
        {
            try
            {
                await _jogoService.ExcluirAsync(id);
                _logger.LogInformation("Jogo com Id {Id} foi desativado.", id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir jogo com Id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }
    }
}