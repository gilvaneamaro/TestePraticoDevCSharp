using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePraticoDevCSharp.App.DTOs;

namespace TestePraticoDevCSharp.App.Services
{
    public class RelatorioVendaService
    {
        private readonly VendaService _vendaService;

        public RelatorioVendaService(VendaService vendaService)
        {
            _vendaService = vendaService;
        }

        
    }
}
