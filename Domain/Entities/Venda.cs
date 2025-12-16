using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.Domain.Entities
{
    public class Venda
    {
        private readonly List<ItemVenda> _itens = new List<ItemVenda>();


        public int Id { get; private set; }
        public int ClienteId { get; private set; }
        public DateTime DataVenda { get; private set; }

        public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();
        public decimal Total => _itens.Sum(i => i.PrecoTotal);

        protected Venda() { }

        public Venda(int clienteId)
        {
            ClienteId = clienteId;
            DataVenda = DateTime.Now;
        }

        public void AdicionarItem(Produto produto, int quantidade)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            produto.RemoverEstoque(quantidade);

            _itens.Add(new ItemVenda(
                produto.Id,
                produto.Nome,
                quantidade,
                produto.Preco
            ));
        }
    }
}