using FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Domain.Entities.FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Domain.Interfaces;
using MongoDB.Driver;

namespace FCG.CatalogAPI.Infrastructure.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly IMongoCollection<Jogo> _jogosCollection;
        private readonly IMongoDatabase _database;

        public JogoRepository(IMongoDatabase database)
        {
            _database = database;
            _jogosCollection = _database.GetCollection<Jogo>("jogos");
        }

        private async Task<int> ObterProximoIdAsync(string nomeSequencia)
        {
            var colecaoContadores = _database.GetCollection<Contador>("contadores");

            var filter = Builders<Contador>.Filter.Eq(c => c.Id, nomeSequencia);
            var update = Builders<Contador>.Update.Inc(c => c.Valor, 1);
            var options = new FindOneAndUpdateOptions<Contador>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var resultado = await colecaoContadores.FindOneAndUpdateAsync(filter, update, options);
            return resultado.Valor;
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            return await _jogosCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Jogo?> ObterPorIdAsync(int idJogo)
        {
            return await _jogosCollection.Find(j => j.IdJogo == idJogo).FirstOrDefaultAsync();
        }

        public async Task AdicionarAsync(Jogo jogo)
        {
            jogo.IdJogo = await ObterProximoIdAsync("jogo_id");
            await _jogosCollection.InsertOneAsync(jogo);
        }

        public async Task AtualizarAsync(Jogo jogo)
        {
            await _jogosCollection.ReplaceOneAsync(j => j.IdJogo == jogo.IdJogo, jogo);
        }
    }
}