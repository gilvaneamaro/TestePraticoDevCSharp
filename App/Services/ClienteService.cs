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

        public async Task Cadastrar(string nome, string email, string telefone)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var emailVO = new Email(email);

                if (_clienteRepository.GetByEmail(emailVO.Endereco) != null)
                    throw new InvalidOperationException("E-mail já cadastrado.");

                var cliente = new Cliente(
                    0,
                    nome,
                    emailVO,
                    telefone
                    );

                await _clienteRepository.Add(cliente);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Atualizar(int id, string nome, string email, string telefone)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var cliente = _clienteRepository.GetById(id);

                if (cliente == null)
                    throw new InvalidOperationException("Cliente não encontrado.");

                var emailVO = new Email(email);

                var existente = _clienteRepository.GetByEmail(emailVO.Endereco);
                foreach ( Cliente c in existente )
                {
                    if (c.Id != id)
                        throw new InvalidOperationException("E-mail já está em uso.");
                }

                var atualizado = new Cliente(
                    cliente.Id,
                    nome,
                    emailVO,
                    telefone);

                _clienteRepository.Update(atualizado);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public List<Cliente> BuscarPorNome(string nome)
        {
            return _clienteRepository.GetByName(nome);
        }

        public List<Cliente> GetByEmail(string email)
        {
            return _clienteRepository.GetByEmail(email);
        }
        
        public void Deletar(int id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente não encontrado.");
                _clienteRepository.Delete(id);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    }
}
