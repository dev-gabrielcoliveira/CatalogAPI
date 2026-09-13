using MongoDB.Bson.Serialization.Attributes;

namespace FCG.CatalogAPI.Domain.Entities
{

    namespace FCG.CatalogAPI.Domain.Entities
    {
        public class Contador
        {
            [BsonId]
            public string Id { get; set; } = string.Empty; 

            [BsonElement("valor")]
            public int Valor { get; set; }
        }
    }
}
