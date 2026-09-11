using FCG.CatalogAPI.Domain.Entities;

namespace FCG.CatalogAPI.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task AdicionarAsync(Jogo jogo);
        Task<Jogo?> ObterPorIdAsync(string id);
        Task<IEnumerable<Jogo>> ObterTodosAsync();
        Task AtualizarAsync(Jogo jogo);
    }
}