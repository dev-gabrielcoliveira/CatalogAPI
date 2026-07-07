using FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Application.Interfaces;
using FCG.CatalogAPI.Infrastructure.Persistence;
using FCG.CatalogAPI.Application.Interfaces.Repository;

namespace FCG.CatalogAPI.Infrastructure.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly ApplicationDbContext _context;

        public JogoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Jogo> ObterTodos()
        {
            return _context.Set<Jogo>().ToList();
        }

        public Jogo? ObterPorId(int id)
        {
            return _context.Set<Jogo>().Find(id);
        }

        public void Cadastrar(Jogo jogo)
        {
            _context.Set<Jogo>().Add(jogo);
            _context.SaveChanges();
        }

        public void Alterar(Jogo jogo)
        {
            _context.Set<Jogo>().Update(jogo);
            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            var jogo = ObterPorId(id);
            if (jogo != null)
            {
                _context.Set<Jogo>().Remove(jogo);
                _context.SaveChanges();
            }
        }

    }
}