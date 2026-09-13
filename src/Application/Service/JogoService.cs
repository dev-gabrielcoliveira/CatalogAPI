using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Validators;
using FCG.CatalogAPI.Domain.Entities;
using FCG.CatalogAPI.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FCG.CatalogAPI.Application.Service
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly JogoValidators _validator;
        private readonly IDistributedCache _cache;

        public JogoService(IJogoRepository jogoRepository, IDistributedCache cache)
        {
            _jogoRepository = jogoRepository;
            _validator = new JogoValidators();
            _cache = cache;
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            string cacheKey = "jogos-todos";

            // 1. Tenta buscar no Redis
            var cachedJogos = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedJogos))
            {
                return JsonSerializer.Deserialize<IEnumerable<Jogo>>(cachedJogos) ?? Enumerable.Empty<Jogo>();
            }

            // 2. Se não estiver no cache, busca no MongoDB
            var jogos = await _jogoRepository.ObterTodosAsync();
            var jogosAtivos = jogos.Where(j => j.Situacao == "Ativo").ToList();

            // 3. Salva no Redis com expiração de 5 minutos
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(jogosAtivos), options);

            return jogosAtivos;
        }

        public async Task<Jogo?> ObterPorIdAsync(int idJogo)
        {
            string cacheKey = $"jogo-{idJogo}";

            // 1. Tenta buscar no Redis
            var cachedJogo = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedJogo))
            {
                return JsonSerializer.Deserialize<Jogo>(cachedJogo);
            }

            // 2. Busca no MongoDB
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            var jogoAtivo = jogo?.Situacao == "Ativo" ? jogo : null;

            if (jogoAtivo != null)
            {
                // 3. Salva no Redis
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(jogoAtivo), options);
            }

            return jogoAtivo;
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

            // Invalida o cache da listagem geral após criar um novo jogo
            await _cache.RemoveAsync("jogos-todos");

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

            // Invalida o cache da lista e o cache específico do jogo atualizado
            await _cache.RemoveAsync("jogos-todos");
            await _cache.RemoveAsync($"jogo-{input.IdJogo}");
        }

        public async Task ExcluirAsync(int idJogo)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);

            if (jogo == null)
                throw new ArgumentException("Jogo não encontrado");

            jogo.Situacao = "Removido";

            await _jogoRepository.AtualizarAsync(jogo);

            // Invalida o cache da lista e o cache específico do jogo removido
            await _cache.RemoveAsync("jogos-todos");
            await _cache.RemoveAsync($"jogo-{idJogo}");
        }
    }
}