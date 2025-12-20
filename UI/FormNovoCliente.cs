using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.UI.Navigation;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormNovoCliente : Form
    {
        private readonly FormNavigator _navigator;
        public FormNovoCliente(FormNavigator navigator)
        {
            InitializeComponent();
            _navigator = navigator;
        }

        private void btnCadastrarCliente_Click(object sender, EventArgs e)
        {


            _navigator.Load(new FormVenda());
        }
    }
}
