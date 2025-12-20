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
using TestePraticoDevCSharp.UI.Navigation;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormDefineCliente : Form, INavigable
    {
        private IFormNavigator _navigator;

        public FormDefineCliente()
        {
            InitializeComponent();
        }

        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            _navigator.Navigate<FormNovoCliente>();
        }
    }
}
