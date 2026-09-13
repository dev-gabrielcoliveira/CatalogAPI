using System.Collections.Generic;

namespace CatalogAPI.Tests.Validators
{
    public class JogoValidators
    {
        public bool NomeValido(string nome)
        {
            return !string.IsNullOrWhiteSpace(nome);
        }

        public bool DescricaoValida(string descricao)
        {
            return !string.IsNullOrWhiteSpace(descricao);
        }

        public bool PrecoValido(decimal preco)
        {
            return preco >= 0;
        }

        public bool GenerosValidos(List<string> generos)
        {
            return generos != null && generos.Count > 0;
        }

        public bool PlataformasValidas(List<string> plataformas)
        {
            return plataformas != null && plataformas.Count > 0;
        }
    }
}