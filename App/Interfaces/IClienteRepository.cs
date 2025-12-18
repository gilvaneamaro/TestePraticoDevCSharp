using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.App.Interfaces
{
    public interface IClienteRepository
    {
        void Add(Cliente cliente);
        void Update(Cliente cliente);
        Cliente GetByEmail(string email);
        Cliente GetById(int id);
    }
}
