using FCG.CatalogAPI.Application.Interfaces.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.CatalogAPI.Domain.Entities
{
    public class Jogo
    {
        [BsonId]
        public int IdJogo { get; set; }

        [BsonElement("nome")]
        public required string Nome { get; set; }

        [BsonElement("descricao")]
        public required string Descricao { get; set; }

        [BsonElement("preco")]
        [BsonRepresentation(BsonType.Decimal128)]
        public required decimal Preco { get; set; }

        [BsonElement("situacao")]
        public required string Situacao { get; set; }

        [BsonElement("generos")]
        public List<string> Generos { get; set; } = new();

        [BsonElement("plataformas")]
        public List<string> Plataformas { get; set; } = new();
    }
}
