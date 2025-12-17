using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            Connection = _connectionFactory.Create();
            Connection.Open();
        }

        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction?.Commit();
        }

        public void Rollback()
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
