using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.App.Services;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormVenda : Form, INavigable
    {
        private Cliente _clienteSelecionado;
        private IFormNavigator _navigator;
        private readonly ClienteService _clienteService;
        public FormVenda(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }
        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {

        }
    }
}
