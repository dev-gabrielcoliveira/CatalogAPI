using System.ComponentModel.DataAnnotations;

namespace FCG.CatalogAPI.Application.DTOs
{
    public record JogoCriarInput
    (
        [Required(ErrorMessage = "O nome é obrigatório.")]
        string Nome,

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        string Descricao,

        [Range(0.01, 10000.00, ErrorMessage = "O preço deve ser maior que zero.")]
        decimal Preco,

        List<string> Generos,
        List<string> Plataformas

    )
    {
        // Garante que as listas nunca venham nulas se omitidas no JSON
        public List<string> Generos { get; init; } = Generos ?? [];
        public List<string> Plataformas { get; init; } = Plataformas ?? [];
    };
}
