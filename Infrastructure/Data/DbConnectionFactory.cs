using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;


namespace TestePraticoDevCSharp.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory()
        {
            _connectionString =
                ConfigurationManager
                    .ConnectionStrings["PostgresConnection"]
                    .ConnectionString;
        }

        public IDbConnection Create()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
