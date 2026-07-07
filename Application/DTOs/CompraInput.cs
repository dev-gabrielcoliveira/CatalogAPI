namespace FCG.CatalogAPI.Application.DTOs
{
    public record CompraInput
    (
        int IdUsuario,
        int IdJogo,
        decimal Preco
    );
}
