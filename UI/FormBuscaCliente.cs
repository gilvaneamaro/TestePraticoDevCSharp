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
        private Cliente _clienteSelecionado;

        public FormBuscaCliente(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }
        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }
        private void FormBuscaCliente_Load(object sender, EventArgs e)
        {
            dgvListaCliente.EditMode = DataGridViewEditMode.EditOnEnter;

            cbTipoPesquisa.Text = "Nome";
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {

            if (cbTipoPesquisa.SelectedItem == null)
            {
                MessageBox.Show(
                    "Selecione um tipo de pesquisa.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (cbTipoPesquisa.SelectedItem.ToString() == "Nome" )
            {
                List<Cliente> clientes = await _clienteService.BuscarPorNomeAsync(txtBusca.Text);

                bsListaCliente.DataSource = new BindingList<Cliente>(clientes);

            }

            if (cbTipoPesquisa.SelectedItem.ToString() == "E-mail")
            {
                var clientes = await _clienteService.GetByEmailAsync(txtBusca.Text);
                bsListaCliente.DataSource = new BindingList<Cliente>(clientes);
            }
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            var cliente = bsListaCliente.Current as Cliente;
            if (cliente == null)
            {
                MessageBox.Show(
                    "Nenhum cliente selecionado.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            _navigator.Navigate<FormVenda>(form =>
            {
                if (form is IClienteSelecionavel receptor)
                    receptor.SetCliente(cliente);
            });
        }

        private async void btnDeletar_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show(
                "Deseja realmente deletar o cliente selecionado?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resposta != DialogResult.Yes)
                return;

            _clienteSelecionado = (Cliente)bsListaCliente.Current;

            await _clienteService.DeletarAsync(_clienteSelecionado.Id);
            bsListaCliente.RemoveCurrent();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show("Deseja realmente editar este registro?", "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (resposta != DialogResult.Yes)
                return;

            if (bsListaCliente.Current == null)
                return;    

            dgvListaCliente.ReadOnly = false;

            foreach (DataGridViewRow row in dgvListaCliente.Rows)
                row.ReadOnly = true;

            dgvListaCliente.CurrentRow.ReadOnly = false;
            btnSalvar.Enabled = true;
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            dgvListaCliente.CommitEdit(DataGridViewDataErrorContexts.Commit);
            dgvListaCliente.EndEdit();
            bsListaCliente.EndEdit();

            var cliente = bsListaCliente.Current as Cliente;
            if (cliente == null)
                return;

            await _clienteService.AtualizarAsync(
                cliente.Id,
                cliente.Nome,
                cliente.EmailEndereco,
                cliente.Telefone
            );

            dgvListaCliente.ReadOnly = true;
            btnSalvar.Enabled = false;

            MessageBox.Show("Cliente atualizado com sucesso!");
        }
    }
}
