using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.App.Interfaces
{

    public interface IUnitOfWork : IDisposable
    {
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }

        Task BeginTransactionAsync();
        Task Commit();
        Task Rollback();
    }
}
