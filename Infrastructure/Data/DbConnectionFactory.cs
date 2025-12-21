using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public DbConnection Create()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
