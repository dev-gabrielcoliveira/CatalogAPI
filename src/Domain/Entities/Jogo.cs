using FCG.CatalogAPI.Application.Interfaces.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.CatalogAPI.Domain.Entities
{
    public class Jogo
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Sobrescreve ou define o ID como string para o MongoDB

        [BsonElement("nome")]
        public required string Nome { get; set; }

        [BsonElement("descricao")]
        public required string Descricao { get; set; }

        [BsonElement("preco")]
        public required decimal Preco { get; set; }

        [BsonElement("situacao")]
        public required string Situacao { get; set; }

        // Vantagem NoSQL: Listas embutidas diretamente no documento do jogo
        [BsonElement("generos")]
        public List<string> Generos { get; set; } = new();

        [BsonElement("plataformas")]
        public List<string> Plataformas { get; set; } = new();

    }
}
