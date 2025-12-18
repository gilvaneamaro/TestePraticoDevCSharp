using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly UnitOfWork _uow;

        public ClienteRepository(UnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Cliente cliente)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                INSERT INTO clientes (nome, email, telefone)
                VALUES (@nome, @email, @telefone)
                RETURNING id;";

                cmd.Parameters.Add(new NpgsqlParameter("@nome", cliente.Nome));
                cmd.Parameters.Add(new NpgsqlParameter("@email", cliente.Email.Endereco));
                cmd.Parameters.Add(new NpgsqlParameter("@telefone", cliente.Telefone));

                var id = Convert.ToInt32(cmd.ExecuteScalar());

                typeof(Cliente)
                    .GetProperty("Id")
                    .SetValue(cliente, id);
            }
        }

        public void Update(Cliente cliente)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
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

                cmd.ExecuteNonQuery();
            }
        }

        public Cliente GetByEmail(string email)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                SELECT id, nome, email, telefone
                FROM clientes
                WHERE email = @email";

                cmd.Parameters.Add(new NpgsqlParameter("@email", email));

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return Map(reader);
                }
            }
        }
        public Cliente GetById(int id)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
            SELECT id, nome, email, telefone
            FROM clientes
            WHERE id = @id";

                cmd.Parameters.Add(new NpgsqlParameter("@id", id));

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return Map(reader);
                }
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
