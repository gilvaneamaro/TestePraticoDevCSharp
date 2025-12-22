using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.App.Services;
using TestePraticoDevCSharp.Domain.Entities;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormEstoque : Form
    {

        private readonly ProdutoService _produtoService;
        private Produto produto;
        private Produto _produtoSelecionado;
        public FormEstoque(ProdutoService produtoService)
        {
            InitializeComponent();
            _produtoService = produtoService;
        }
        private async void FormEstoque_Load(object sender, EventArgs e)
        {
            List<Produto> produtos = await _produtoService.ObterTodos();
            bsListaEstoque = new BindingSource(produtos, null);
            dgvEstoque.DataSource = bsListaEstoque;
        }

        private async void btnSalvarProduto_Click(object sender, EventArgs e)
        {
            try 
            { 
                produto = await _produtoService.AdicionarProduto(
                    txtNomeProduto.Text,
                    txtDescricaoProduto.Text,
                    decimal.Parse(txtValorUnitario.Text),
                    int.Parse(nupEstoque.Text)
                );

                txtNomeProduto.Text = "";
                txtDescricaoProduto.Text = "";
                txtValorUnitario.Text = "";
                nupEstoque.Text = "";


                var produtosAtualizados = await _produtoService.ObterTodos();
                bsListaEstoque.DataSource = produtosAtualizados;
                bsListaEstoque.ResetBindings(false);

                MessageBox.Show(
                    "Produto cadastrado com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        private void btnEditar_Click(object sender, EventArgs e)
        {

        }

        private async void btnDeletar_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show(
                "Tem certeza que deseja deletar o produto selecionado?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (resposta != DialogResult.Yes)
                return;
            _produtoSelecionado = (Produto)bsListaEstoque.Current;

            await _produtoService.Deletar(_produtoSelecionado.Id);
            bsListaEstoque.RemoveCurrent();
        }

        private void txtValorUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && txtValorUnitario.Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void txtValorUnitario_Leave(object sender, EventArgs e)
        {
            var culture = new CultureInfo("pt-BR");

            if (decimal.TryParse(txtValorUnitario.Text, NumberStyles.Any, culture, out decimal valor))
            {
                txtValorUnitario.Text = valor.ToString("N2", culture);
            }
            else
            {
                txtValorUnitario.Text = "0,00";
            }
        }
    }
}
