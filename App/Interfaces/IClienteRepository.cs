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

        Task UpdateAsync(Cliente cliente);

        Task<Cliente> GetByIdAsync(int id);

        Task<List<Cliente>> GetByEmailAsync(string email);

        Task<List<Cliente>> GetByNameAsync(string nome);

        Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null);

        Task DeleteAsync(int id);
    }
}
