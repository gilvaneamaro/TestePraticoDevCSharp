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
using TestePraticoDevCSharp.UI.Navigation;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormNovoCliente : Form, INavigable
    {
        private IFormNavigator _navigator;
        private readonly ClienteService _clienteService;

        public FormNovoCliente(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }
        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }

        private async void btnCadastrarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                btnCadastrarCliente.Enabled = false;

                await _clienteService.Cadastrar(
                    txtNome.Text,
                    txtEmail.Text,
                    txtTelefone.Text
                );

                MessageBox.Show(
                    "Cliente cadastrado com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                _navigator.Navigate<FormVenda>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnCadastrarCliente.Enabled = true;
            }
        }
    }
}
