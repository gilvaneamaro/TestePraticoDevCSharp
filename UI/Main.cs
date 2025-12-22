using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.UI;
using TestePraticoDevCSharp.UI.Navigation;

namespace TestePraticoDevCSharp
{
    public partial class Main : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private FormNavigator _navigator;

        public Main(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _navigator = new FormNavigator(pnlMain, _serviceProvider);
            _navigator.Navigate<FormDefineCliente>();
        }

        private void vendaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cadastrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            _navigator.Navigate<FormEstoque>();
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            _navigator.Navigate<FormDefineCliente>();
        }
    }
}
