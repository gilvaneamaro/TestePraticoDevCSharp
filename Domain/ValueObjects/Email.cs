using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }
        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco) || !endereco.Contains("@"))
            {
                throw new ArgumentException("Endereço de email inválido.");
            }
            Endereco = endereco;
        }

        public override bool Equals(object obj)
            => obj is Email email && Endereco == email.Endereco;

        public override int GetHashCode()
            => Endereco.GetHashCode();

        public override string ToString()
        {
            return Endereco;
        }
    }

}
