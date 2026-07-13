using CatalogAPI.Tests.Validators;

namespace CatalogAPI.Tests;

public class JogoValidatorsTests
{
    private readonly JogoValidators _validator = new();

    [Fact]
    public void Nome_Valido()
    {
        Assert.True(_validator.NomeValido("God of War"));
    }

    [Fact]
    public void Nome_Invalido()
    {
        Assert.False(_validator.NomeValido(""));
    }

    [Fact]
    public void Descricao_Valida()
    {
        Assert.True(_validator.DescricaoValida("Jogo de ação"));
    }

    [Fact]
    public void Descricao_Invalida()
    {
        Assert.False(_validator.DescricaoValida(""));
    }

    [Fact]
    public void Preco_Valido()
    {
        Assert.True(_validator.PrecoValido(199.90m));
    }

    [Fact]
    public void Preco_Invalido()
    {
        Assert.False(_validator.PrecoValido(-10));
    }
}