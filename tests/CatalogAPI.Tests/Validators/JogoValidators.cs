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
    }
}