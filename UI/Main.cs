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
        private Form _currentForm;
        private FormNavigator _navigator;

        public Main()
        {
            InitializeComponent();
            _navigator = new FormNavigator(pnlMain);
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void vendaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cadastrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bntCaixa_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            _navigator.Load(new FormCliente());
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            _navigator.Load(new FormEstoque());
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            _navigator.Load(new FormRelatorio());
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            _navigator.Load(new FormDefineCliente(_navigator));
        }
    }
}
