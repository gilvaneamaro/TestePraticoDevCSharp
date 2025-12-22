using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.App.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProdutoService(
            IProdutoRepository produtoRepository,
            IUnitOfWork unitOfWork)
        {
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Produto> AdicionarProduto(
     string nome,
     string descricao,
     decimal preco,
     int estoque)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var produto = new Produto(nome, preco, descricao, estoque);

                await _produtoRepository.Adicionar(produto);
                await _unitOfWork.Commit();

                return produto;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
   

        public async Task<List<Produto>> ObterTodos()
        {
            await _unitOfWork.BeginTransactionAsync();


            List<Produto> produtos = await _produtoRepository.ListarTodosAsync();
            await _unitOfWork.Commit();

            return produtos;
        }

        public async Task Deletar(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
  
                Produto produto = await _produtoRepository.ObterPorIdAsync(id);
                if (produto == null)
                    throw new ArgumentException("Produto não encontrado.");

                await _produtoRepository.RemoverAsync(id);
                await _unitOfWork.Commit();
            
        }
        public async Task AtualizarProduto(Produto produto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existente = await _produtoRepository.ObterPorIdAsync(produto.Id);
                if (existente == null)
                    throw new ArgumentException("Produto não encontrado.");

                await _produtoRepository.AtualizarAsync(produto);
                await _unitOfWork.Commit();
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
