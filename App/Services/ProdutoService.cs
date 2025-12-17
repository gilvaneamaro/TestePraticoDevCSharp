using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.App.Services
{
    public class ProdutoService
    {
        public void AdicionarProduto(string nome, decimal preco, int estoque)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do produto é obrigatório.");
            if (preco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.");
            if (estoque < 0)
                throw new ArgumentException("Estoque não pode ser negativo.");


        }

    }
}
