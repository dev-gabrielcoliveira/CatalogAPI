using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Domain.Entities;

namespace FCG.CatalogAPI.Application.Interfaces.Service
{
    public interface IJogoService
    {
        Task<IEnumerable<Jogo>> ObterTodosAsync(); 
        Task<Jogo?> ObterPorIdAsync(string id);       
        Task<Jogo> CriarAsync(JogoCriarInput input);
        Task AtualizarAsync(JogoAtualizarInput input);
        Task ExcluirAsync(string id);
    }
}
