using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoDevCSharp.App.DTOs
{
    public class RelatorioVendaClienteDto
    {
        public int VendaId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorItem { get; set; }
    }
}
