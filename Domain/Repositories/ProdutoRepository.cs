using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.Domain.Entities;
using TestePraticoDevCSharp.Domain.ValueObjects;

namespace TestePraticoDevCSharp.Domain.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProdutoRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Adicionar(Produto produto)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                INSERT INTO produtos(nome, descricao, preco, estoque)
                VALUES (@nome, @descricao, @preco, @estoque)
                RETURNING id;";
                cmd.Parameters.Add(new NpgsqlParameter("@nome", produto.Nome));
                cmd.Parameters.Add(new NpgsqlParameter("@descricao", produto.Descricao));
                cmd.Parameters.Add(new NpgsqlParameter("@preco", produto.Preco));
                cmd.Parameters.Add(new NpgsqlParameter("@estoque", produto.Estoque));
            
                var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                typeof(Produto)
                    .GetProperty("Id")
                    .SetValue(produto, id);
            }
        }

        public async Task AtualizarAsync(Produto produto)
        {
            throw new NotImplementedException();
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, descricao, preco, estoque
                FROM produtos
                WHERE id = @id";



                cmd.Parameters.Add(new NpgsqlParameter("@id", id));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                        return null;

                    return Map(reader);
                }
            }
        }

        public async Task<List<Produto>> ObterPorNomeAsync(string nome)
        {
            throw new NotImplementedException();
        }

        public async Task AdicionaEstoque(int produtoId, int quantidade)
        {
            throw new NotImplementedException();
        }
        public async Task RemoverAsync(int id)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                UPDATE produtos
                SET ativo = FALSE
                WHERE id = @id";

                cmd.Parameters.Add(new NpgsqlParameter("@id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<Produto>> ListarTodosAsync()
        {
            var produtos = new List<Produto>();
            if (_unitOfWork.Connection.State != ConnectionState.Open)
                _unitOfWork.Connection.Open();

            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, descricao, preco, estoque
                FROM produtos
                WHERE ativo=true;";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        produtos.Add(Map(reader));
                    }
                }
            }

            return produtos;
        }

        public async Task BaixarEstoque(int produtoId, int quantidade)
        {
            using (var cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                UPDATE produtos
                SET estoque = estoque - @quantidade
                WHERE id = @produtoId
                AND estoque >= @quantidade;";

                cmd.Parameters.Add(new NpgsqlParameter("@produtoId", produtoId));
                cmd.Parameters.Add(new NpgsqlParameter("@quantidade", quantidade));

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                    throw new InvalidOperationException("Estoque insuficiente.");
            }
        }

        private Produto Map(IDataReader reader)
        {
            return new Produto(
                reader.GetInt32(0),     
                reader.GetString(1),   
                reader.GetString(2),   
                reader.GetInt32(3),  
                reader.GetInt32(4) 
            );
        }
    }
}


