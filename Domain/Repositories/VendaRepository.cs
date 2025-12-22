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
    public class VendaRepository : IVendaRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendaRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task RegistrarVenda(Venda venda)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                INSERT INTO vendas (cliente_id, data_venda, metodo_pagamento, valor_total)
                VALUES (@clienteId, @dataVenda, @metodoPagamento, @valorTotal)
                RETURNING id;
            ";

                cmd.Parameters.Add(new NpgsqlParameter("@clienteId", venda.ClienteId));
                cmd.Parameters.Add(new NpgsqlParameter("@dataVenda", venda.DataVenda));
                cmd.Parameters.Add(new NpgsqlParameter("@metodoPagamento", (int)venda.MetodoPagamento));
                cmd.Parameters.Add(new NpgsqlParameter("@valorTotal", venda.ValorTotal));

                var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                typeof(Venda)
                    .GetProperty("Id")
                    .SetValue(venda, id);
            }
        }
    }
}
