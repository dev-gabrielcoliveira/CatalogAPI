namespace FCG.CatalogAPI.Application.DTOs
{
    public record JogoAtualizarInput
    (
        string IdJogo,
        string Nome,
        string Descricao,
        decimal Preco 
    );
}
