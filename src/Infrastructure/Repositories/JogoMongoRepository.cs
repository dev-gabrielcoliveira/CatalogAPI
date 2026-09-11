using FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Domain.Interfaces;
using MongoDB.Driver;

namespace FCG.CatalogAPI.Infrastructure.Repositories
{
    public class JogoMongoRepository : IJogoRepository
    {
        private readonly IMongoCollection<Jogo> _jogosCollection;

        // Injetando o IMongoDatabase diretamente que configuramos no Program.cs
        public JogoMongoRepository(IMongoDatabase database)
        {
            _jogosCollection = database.GetCollection<Jogo>("Jogos");
        }

        public async Task AdicionarAsync(Jogo jogo)
        {
            await _jogosCollection.InsertOneAsync(jogo);
        }

        public async Task<Jogo?> ObterPorIdAsync(string id)
        {
            return await _jogosCollection.Find(j => j.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            return await _jogosCollection.Find(_ => true).ToListAsync();
        }

        public async Task AtualizarAsync(Jogo jogo)
        {
            // Substitui o documento existente no banco que tenha o mesmo ID
            await _jogosCollection.ReplaceOneAsync(j => j.Id == jogo.Id, jogo);
        }
    }
}