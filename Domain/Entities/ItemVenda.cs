using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.Domain.Entities
{
    public class ItemVenda
    {
        public int Id { get; private set; }
        public int ProdutoId { get; private set; }
        public string Produto { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        public decimal PrecoTotal => Quantidade * PrecoUnitario;

        protected ItemVenda() { }

        public ItemVenda(int produtoId, string produto, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");
            if (precoUnitario <= 0)
                throw new ArgumentException("Preço unitário deve ser maior que zero.");
            ProdutoId = produtoId;
            Produto = produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public void AumentarQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");
            Quantidade += quantidade;
        }
    }
}
