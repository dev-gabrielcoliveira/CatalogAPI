using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Validators;
using FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Domain.Interfaces;

namespace FCG.CatalogAPI.Application.Service
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly JogoValidators _validator;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
            _validator = new JogoValidators();
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            var jogos = await _jogoRepository.ObterTodosAsync();
            return jogos.Where(j => j.Situacao == "Ativo");
        }

        public async Task<Jogo?> ObterPorIdAsync(int idJogo)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            return jogo?.Situacao == "Ativo" ? jogo : null;
        }

        public async Task<Jogo> CriarAsync(JogoCriarInput input)
        {
            if (!_validator.NomeValido(input.Nome))
                throw new ArgumentException("Nome inválido");

            if (!_validator.DescricaoValida(input.Descricao))
                throw new ArgumentException("Descrição inválida");

            if (!_validator.PrecoValido(input.Preco))
                throw new ArgumentException("Preço inválido");

            if (!_validator.GenerosValidos(input.Generos))
                throw new ArgumentException("Gêneros inválidos ou vazios");

            if (!_validator.PlataformasValidas(input.Plataformas))
                throw new ArgumentException("Plataformas inválidas ou vazias");

            var jogo = new Jogo
            {
                Nome = input.Nome,
                Descricao = input.Descricao,
                Preco = input.Preco,
                Situacao = "Ativo",
                Generos = input.Generos,
                Plataformas = input.Plataformas
            };

            await _jogoRepository.AdicionarAsync(jogo);

            return jogo;
        }
        public async Task AtualizarAsync(JogoAtualizarInput input)
        {
            if (!_validator.NomeValido(input.Nome))
                throw new ArgumentException("Nome inválido");

            if (!_validator.DescricaoValida(input.Descricao))
                throw new ArgumentException("Descrição inválida");

            if (!_validator.PrecoValido(input.Preco))
                throw new ArgumentException("Preço inválido");

            if (!_validator.GenerosValidos(input.Generos))
                throw new ArgumentException("Gêneros inválidos ou vazios");

            if (!_validator.PlataformasValidas(input.Plataformas))
                throw new ArgumentException("Plataformas inválidas ou vazias");

            var jogo = await _jogoRepository.ObterPorIdAsync(input.IdJogo);

            if (jogo == null)
                throw new ArgumentException("Jogo não encontrado");

            jogo.Descricao = input.Descricao;
            jogo.Nome = input.Nome;
            jogo.Preco = input.Preco;
            jogo.Generos = input.Generos;
            jogo.Plataformas = input.Plataformas;

            await _jogoRepository.AtualizarAsync(jogo);
        }

        public async Task ExcluirAsync(int idJogo)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);

            if (jogo == null)
                throw new ArgumentException("Jogo não encontrado");

            jogo.Situacao = "Removido";

            await _jogoRepository.AtualizarAsync(jogo);
        }
    }
}