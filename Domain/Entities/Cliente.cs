using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.Domain.ValueObjects;

namespace TestePraticoDevCSharp.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string Telefone { get; private set; }

        public bool Ativo { get; private set; }
        public string EmailEndereco
        {
            get => Email.Endereco;
            set => AlterarEmail(value);
        }

        public void AlterarNome(string nome)
        {
            Nome = nome;
        }

        public void AlterarTelefone(string telefone)
        {
            Telefone = telefone;
        }

        public void AlterarEmail(string email)
        {
            Email = new Email(email);
        }

        public string NomeEdicao
        {
            get => Nome;
            set => AlterarNome(value);
        }

        public string TelefoneEdicao
        {
            get => Telefone;
            set => AlterarTelefone(value);
        }

        public Cliente(int id, string nome, Email email, string telefone, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Ativo = ativo;
        }

        public Cliente(int id, string nome, Email email, string telefone)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }
        public Cliente(string nome, Email email, string telefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }
        protected Cliente() { }

    }
}
