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
        Task Add(Cliente cliente);
        void Update(Cliente cliente);
        List<Cliente> GetByEmail(string email);
        Cliente GetById(int id);
        List<Cliente> GetByName(string nome);
        void Delete(int id);
    }
}
