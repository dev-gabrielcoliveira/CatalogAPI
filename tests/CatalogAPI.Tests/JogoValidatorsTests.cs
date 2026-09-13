using CatalogAPI.Tests.Validators;
using System.Collections.Generic;
using Xunit;

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

    [Fact]
    public void Generos_Validos()
    {
        var generos = new List<string> { "Ação", "RPG" };
        Assert.True(_validator.GenerosValidos(generos));
    }

    [Fact]
    public void Generos_Invalidos()
    {
        var generosVazio = new List<string>();
        Assert.False(_validator.GenerosValidos(generosVazio));
    }

    [Fact]
    public void Plataformas_Validas()
    {
        var plataformas = new List<string> { "PC", "PlayStation 5" };
        Assert.True(_validator.PlataformasValidas(plataformas));
    }

    [Fact]
    public void Plataformas_Invalidas()
    {
        var plataformasVazio = new List<string>();
        Assert.False(_validator.PlataformasValidas(plataformasVazio));
    }
}