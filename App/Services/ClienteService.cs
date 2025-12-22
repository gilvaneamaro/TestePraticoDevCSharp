using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.Domain.Entities;
using TestePraticoDevCSharp.Domain.ValueObjects;

namespace TestePraticoDevCSharp.App.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(
            IClienteRepository clienteRepository,
            IUnitOfWork unitOfWork)
        {
            _clienteRepository = clienteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Cliente> CadastrarAsync(string nome, string email, string telefone)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var emailVO = new Email(email);

                if (await _clienteRepository.ExistsByEmailAsync(emailVO.Endereco))
                    throw new InvalidOperationException("E-mail já cadastrado.");

                var cliente = new Cliente(
                    nome,
                    emailVO,
                    telefone
                );

                await _clienteRepository.Add(cliente);
                await _unitOfWork.Commit();

                return cliente;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }


        public async Task AtualizarAsync(int id, string nome, string email, string telefone)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente == null)
                    throw new InvalidOperationException("Cliente não encontrado.");

                var emailVO = new Email(email);

                if (await _clienteRepository.ExistsByEmailAsync(emailVO.Endereco, id))
                    throw new InvalidOperationException("E-mail já está em uso.");

                var atualizado = new Cliente(
                    cliente.Id,
                    nome,
                    emailVO,
                    telefone
                );

                await _clienteRepository.UpdateAsync(atualizado);

                await _unitOfWork.Commit();
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<List<Cliente>> BuscarPorNomeAsync(string nome)
        {
            return await _clienteRepository.GetByNameAsync(nome);
        }

        public async Task<List<Cliente>> GetByEmailAsync(string email)
        {
            return await _clienteRepository.GetByEmailAsync(email);
        }


        public async Task DeletarAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente == null)
                    throw new InvalidOperationException("Cliente não encontrado.");

                await _clienteRepository.DeleteAsync(id);

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
