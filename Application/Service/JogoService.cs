using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Interfaces.Repository;
using FCG.CatalogAPI.Domain.Entities;

namespace FCG.CatalogAPI.Application.Service
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public void Atualizar(JogoAtualizarInput input)
        {
            ValidarDadosJogo(input.Nome, input.Descricao, input.Preco);

            var jogo = this.ObterPorId(input.IdJogo);

            if (jogo == null)
                throw new ArgumentException("Jogo não encontrado");

            jogo.Descricao = input.Descricao;
            jogo.Nome = input.Nome;
            jogo.Preco = input.Preco;

            _jogoRepository.Alterar(jogo);
        }

        public Jogo Criar(JogoCriarInput input)
        {
            ValidarDadosJogo(input.Nome, input.Descricao, input.Preco);

            var jogo = new Jogo
            {
                Nome = input.Nome,
                Descricao = input.Descricao,
                Preco = input.Preco,
                Situacao = "Ativo"
            };

            _jogoRepository.Cadastrar(jogo);

            return jogo;
        }

        public void Excluir(int id)
        {
            var jogo = ObterPorId(id);

            if (jogo == null)
                throw new ArgumentException("Jogo não encontrado");

            jogo.Situacao = "Removido";

            _jogoRepository.Alterar(jogo);
        }

        public Jogo? ObterPorId(int id)
        {
            return _jogoRepository.ObterTodos()
                .FirstOrDefault(j => j.Id == id && j.Situacao == "Ativo");
        }

        public IEnumerable<Jogo> ObterTodos()
        {
            return _jogoRepository.ObterTodos()
                .Where(j => j.Situacao == "Ativo");
        }

        private static void ValidarDadosJogo(string? nome, string? descricao, decimal preco)
        {
            if (preco < 0)
                throw new ArgumentException("Preço inválido.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome inválido");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição inválida");
        }
    }
}