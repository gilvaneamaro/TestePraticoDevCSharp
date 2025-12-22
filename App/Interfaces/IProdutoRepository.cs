using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.App.Interfaces
{
    public interface IProdutoRepository
    {
        Task Adicionar(Produto produto);

        Task AtualizarAsync(Produto produto);

        Task<Produto> ObterPorIdAsync(int id);

        Task<List<Produto>> ObterPorNomeAsync(string nome);

        Task AdicionaEstoque(int produtoId, int quantidade);

        Task RemoverAsync(int id);
        Task<List<Produto>> ListarTodosAsync();
    }
}
