using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;

namespace TestePraticoDevCSharp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbConnection _connection;

        public DbConnection Connection => _connection;
        public DbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.Create();
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            Transaction = _connection.BeginTransaction();
        }

        public async Task Commit()
        {
            Transaction?.Commit();
        }

        public async Task Rollback()
        {
            Transaction?.Rollback();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}
