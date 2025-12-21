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
using TestePraticoDevCSharp.Infrastructure.Data;

namespace TestePraticoDevCSharp.Domain.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Cliente cliente)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                INSERT INTO clientes (nome, email, telefone)
                VALUES (@nome, @email, @telefone)
                RETURNING id;";

                cmd.Parameters.Add(new NpgsqlParameter("@nome", cliente.Nome));
                cmd.Parameters.Add(new NpgsqlParameter("@email", cliente.Email.Endereco));
                cmd.Parameters.Add(new NpgsqlParameter("@telefone", cliente.Telefone));

                var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                typeof(Cliente)
                    .GetProperty("Id")
                    .SetValue(cliente, id);
            }
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                UPDATE clientes
                SET nome = @nome,
                    email = @email,
                    telefone = @telefone
                WHERE id = @id";

                cmd.Parameters.Add(new NpgsqlParameter("@nome", cliente.Nome));
                cmd.Parameters.Add(new NpgsqlParameter("@email", cliente.Email.Endereco));
                cmd.Parameters.Add(new NpgsqlParameter("@telefone", cliente.Telefone));
                cmd.Parameters.Add(new NpgsqlParameter("@id", cliente.Id));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, email, telefone
                FROM clientes
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

        public async Task<List<Cliente>> GetByEmailAsync(string email)
        {
            var clientes = new List<Cliente>();
            if (_unitOfWork.Connection.State != ConnectionState.Open)
                _unitOfWork.Connection.Open();

            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, email, telefone
                FROM clientes
                WHERE email ILIKE @email AND ativo=true";

                cmd.Parameters.Add(new NpgsqlParameter("@email", $"%{email}%"));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clientes.Add(Map(reader));
                    }
                }
            }

            return clientes;
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;

                cmd.CommandText = ignoreId.HasValue
                    ? @"SELECT 1 FROM clientes WHERE email = @email AND id <> @id LIMIT 1"
                    : @"SELECT 1 FROM clientes WHERE email = @email LIMIT 1";

                cmd.Parameters.Add(new NpgsqlParameter("@email", email));

                if (ignoreId.HasValue)
                    cmd.Parameters.Add(new NpgsqlParameter("@id", ignoreId.Value));

                var result = await cmd.ExecuteScalarAsync();
                return result != null;
            }
        }

        public async Task<List<Cliente>> GetByNameAsync(string nome)
        {
            var clientes = new List<Cliente>();

            if (_unitOfWork.Connection.State != ConnectionState.Open)
                _unitOfWork.Connection.Open();

            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, email, telefone
                FROM clientes
                WHERE nome ILIKE @nome AND ativo=true
                ORDER BY nome";

                cmd.Parameters.Add(new NpgsqlParameter("@nome", $"%{nome}%"));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clientes.Add(Map(reader));
                    }
                }
            }

            return clientes;
        }


        public async Task DeleteAsync(int id)
        {
            using (DbCommand cmd = _unitOfWork.Connection.CreateCommand())
            {
                cmd.Transaction = _unitOfWork.Transaction;
                cmd.CommandText = @"
                UPDATE clientes
                SET ativo = FALSE
                WHERE id = @id";

                cmd.Parameters.Add(new NpgsqlParameter("@id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private Cliente Map(IDataReader reader)
        {
            return new Cliente(
                reader.GetInt32(0),
                reader.GetString(1),
                new Email(reader.GetString(2)),
                reader.GetString(3)
            );
        }
    }
}
