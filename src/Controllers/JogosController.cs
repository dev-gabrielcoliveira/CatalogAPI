using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FCG.CatalogAPI.Controllers
{
    /// <summary>
    /// Responsável por gerenciar o catálogo de jogos da plataforma.
    /// </summary>
    /// <remarks>
    /// Permite listar, buscar, cadastrar, atualizar e remover jogos.
    /// </remarks>
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

        /// <summary>
        /// Busca todos os jogos ativos.
        /// </summary>
        /// <returns>Listagem de todos os jogos ativos no sistema.</returns>
        [HttpGet]
        [Authorize(Policy = "AdministradorOuUsuario")] // Usuários comuns também listam jogos
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Jogo>> ObterTodos()
        {
            try
            {
                var jogos = _jogoService.ObterTodos();
                return Ok(jogos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os jogos.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }

        /// <summary>
        /// Busca um jogo específico pelo ID.
        /// </summary>
        /// <param name="id">Identificador do jogo.</param>
        /// <returns>Dados do jogo solicitado.</returns>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "AdministradorOuUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ObterPorId([FromRoute, Range(1, int.MaxValue)] int id)
        {
            try
            {
                var jogo = _jogoService.ObterPorId(id);
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

        /// <summary>
        /// Cria um novo jogo no catálogo.
        /// </summary>
        /// <param name="input">Dados do jogo a ser criado.</param>
        /// <returns>O jogo criado com seu respectivo ID.</returns>
        [HttpPost]
        [Authorize(Policy = "Administrador")] // Apenas admin gerencia catálogo
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Criar([FromBody] JogoCriarInput input)
        {

            try
            {
                var jogoCriado = _jogoService.Criar(input);
                _logger.LogInformation("Jogo '{Nome}' criado com sucesso.", input.Nome);

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

        /// <summary>
        /// Atualiza os dados de um jogo existente.
        /// </summary>
        /// <param name="id">Identificador do jogo.</param>
        /// <param name="jogo">Novos dados do jogo.</param>
        /// <returns>Confirmação da atualização sem corpo de retorno.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Atualizar(int id,[FromBody] JogoAtualizarInput inputJogo)
        {
            if (id != inputJogo.IdJogo) // Impede que o jogo errado seja alterado
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do corpo da requisição." });

            var jogo = _jogoService.ObterPorId(id);
            if (jogo == null)
                return NotFound(new { mensagem = "Jogo não encontrado para atualização." });

            try
            {
                _jogoService.Atualizar(inputJogo);
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

        /// <summary>
        /// Remove logicamente um jogo do catálogo.
        /// </summary>
        /// <param name="id">Identificador do jogo.</param>
        /// <returns>Confirmação da remoção.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Remover([FromRoute, Range(1, int.MaxValue)] int id)
        {
            try
            {
                var jogo = _jogoService.ObterPorId(id);
                if (jogo == null)
                    return NotFound(new { mensagem = "Jogo não encontrado." });

                _jogoService.Excluir(id);
                _logger.LogInformation("Jogo com Id {Id} foi desativado (Removido logicamente).", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir jogo com Id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro interno no servidor." });
            }
        }
    }
}