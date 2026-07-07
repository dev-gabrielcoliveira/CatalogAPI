using FCG.CatalogAPI.Application.Interfaces.Base;

namespace FCG.CatalogAPI.Domain.Entities
{
    public class Jogo: EntityBase
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required decimal Preco { get; set; }
        public required string Situacao { get; set; }
    }
}
