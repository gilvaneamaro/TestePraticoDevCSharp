using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.Domain.Entities
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public string Descricao { get; private set; }
        public int Estoque { get; private set; }
        public bool Ativo { get; private set; }

        public Produto(int id, string nome, string descricao, decimal preco, int estoque)

        {
            Id = id;
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Estoque = estoque;
        }

        public Produto(string nome, decimal preco, string descricao, int estoque)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do produto é obrigatório.");

            if (preco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.");

            if (estoque < 0)
                throw new ArgumentException("Estoque não pode ser negativo.");

            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Estoque = estoque;
        }


        protected Produto() { }

        public void AtualizarPreco(decimal novoPreco)
        {
            if (novoPreco <= 0)
                throw new ArgumentException("Preço inválido.");

            Preco = novoPreco;
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade inválida.");

            Estoque += quantidade;
        }

        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade inválida.");

            if (quantidade > Estoque)
                throw new InvalidOperationException("Estoque insuficiente.");

            Estoque -= quantidade;
        }
        public bool PodeVender(int quantidade)
        {
            return quantidade > 0 && quantidade <= Estoque;
        }
    }
}
