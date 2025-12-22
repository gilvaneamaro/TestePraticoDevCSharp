using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.Domain.Enums;

namespace TestePraticoDevCSharp.Domain.Entities
{
    public class Venda
    {
        private readonly List<ItemVenda> _itens = new List<ItemVenda>();

        public int Id { get; private set; }
        public int ClienteId { get; private set; }
        public DateTime DataVenda { get; private set; }
        public MetodoPagamento MetodoPagamento { get; private set; }

        public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();
        public decimal ValorTotal => _itens.Sum(i => i.PrecoTotal);

        protected Venda() { }

        public Venda(int clienteId, MetodoPagamento metodoPagamento)
        {
            ClienteId = clienteId;
            MetodoPagamento = metodoPagamento;
            DataVenda = DateTime.Now;
        }

        public void AdicionarItem(ItemVenda item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itens.Add(item);
        }
    }
}