using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.Domain.Entities;
using TestePraticoDevCSharp.Domain.Enums;

namespace TestePraticoDevCSharp.App.Services
{
    public class VendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IItemVendaRepository _itemVendaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdutoRepository _produtoRepository;

        public VendaService(
            IVendaRepository vendaRepository,
            IItemVendaRepository itemVendaRepository,
            IProdutoRepository produtoRepository,
            IUnitOfWork unitOfWork)
        {
            _vendaRepository = vendaRepository;
            _itemVendaRepository = itemVendaRepository;
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task FinalizarVendaAsync(
            int clienteId,
            MetodoPagamento metodoPagamento,
            List<ItemVenda> itens)
        {
            if (clienteId <= 0)
                throw new ArgumentException("Cliente inválido.");

            if (itens == null || !itens.Any())
                throw new InvalidOperationException("A venda deve possuir ao menos um item.");

            var venda = new Venda(clienteId, metodoPagamento);

            foreach (var item in itens)
            {
                venda.AdicionarItem(item);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {

                foreach (var item in venda.Itens)
                {
                    await _produtoRepository.BaixarEstoque(
                        item.ProdutoId,
                        item.Quantidade
                    );
                }

                await _vendaRepository.RegistrarVenda(venda);

                foreach (var item in venda.Itens)
                {
                    item.DefinirVenda(venda.Id);
                    await _itemVendaRepository.Registrar(item);
                }

                await _unitOfWork.Commit();
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
