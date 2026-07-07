namespace FCG.CatalogAPI.Application.DTOs
{
    public record JogoCriarInput
    (
        string Nome,
        string Descricao,
        decimal Preco
    );
}
