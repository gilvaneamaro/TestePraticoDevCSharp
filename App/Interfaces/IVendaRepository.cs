using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.App.Interfaces
{
    public interface IVendaRepository
    {
        Task RegistrarVenda(Venda venda);
    }
}
