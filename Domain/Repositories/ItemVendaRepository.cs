using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.Domain.Repositories
{
    public class ItemVendaRepository : IItemVendaRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemVendaRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Registrar(ItemVenda itemVenda)
        {
            using (var cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
            INSERT INTO venda_itens (venda_id, produto_id,quantidade, preco_unitario)
            VALUES (@vendaId, @produtoId, @quantidade, @precoUnitario);
        ";

                cmd.Parameters.Add(new NpgsqlParameter("@vendaId", itemVenda.VendaId));
                cmd.Parameters.Add(new NpgsqlParameter("@produtoId", itemVenda.ProdutoId));
                cmd.Parameters.Add(new NpgsqlParameter("@quantidade", itemVenda.Quantidade));
                cmd.Parameters.Add(new NpgsqlParameter("@precoUnitario", itemVenda.PrecoUnitario));

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

}
