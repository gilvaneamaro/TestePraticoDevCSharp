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
    public partial class FormBuscaCliente : Form, INavigable
    {
        private IFormNavigator _navigator;
        private readonly ClienteService _clienteService;
        public FormBuscaCliente(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }
        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {

                if (cbTipoPesquisa.SelectedItem.ToString() == "Nome")
                {
                    bsListaCliente.DataSource = _clienteService.BuscarPorNome(txtBusca.Text);
                }
                if (cbTipoPesquisa.SelectedItem.ToString() == "E-mail")
                {
                    bsListaCliente.DataSource = _clienteService.GetByEmail(txtBusca.Text);
                }


        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {

        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "Deseja realmente deletar o cliente selecionado?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado != DialogResult.Yes)
                return;

            var cliente = (Cliente)bsListaCliente.Current;
            _clienteService.Deletar(cliente.Id);
        }
    }
}
