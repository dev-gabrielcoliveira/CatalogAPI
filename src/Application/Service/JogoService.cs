using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Interfaces.Repository;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Validators;
using FCG.CatalogAPI.Domain.Entities;

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

        public void Atualizar(JogoAtualizarInput input)
        {
            if (!_validator.NomeValido(input.Nome))
                throw new ArgumentException("Nome inválido");

            if (!_validator.DescricaoValida(input.Descricao))
                throw new ArgumentException("Descrição inválida");

            if (!_validator.PrecoValido(input.Preco))
                throw new ArgumentException("Preço inválido");

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
            if (!_validator.NomeValido(input.Nome))
                throw new ArgumentException("Nome inválido");

            if (!_validator.DescricaoValida(input.Descricao))
                throw new ArgumentException("Descrição inválida");

            if (!_validator.PrecoValido(input.Preco))
                throw new ArgumentException("Preço inválido");

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

    }
}