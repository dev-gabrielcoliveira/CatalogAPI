namespace FCG.CatalogAPI.Application.DTOs
{
    public record JogoAtualizarInput
    (
        int IdJogo,
        string Nome,
        string Descricao,
        decimal Preco 
    );
}
