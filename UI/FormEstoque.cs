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
                if (_produtoSelecionado == null)
                {
                    await _produtoService.AdicionarProduto(
                        txtNomeProduto.Text,
                        txtDescricaoProduto.Text,
                        decimal.Parse(txtValorUnitario.Text),
                        (int)nupEstoque.Value
                    );
                }
                else
                {
                    AtualizarProdutoSelecionado();
                    await _produtoService.AtualizarProduto(_produtoSelecionado);
                }

                LimparFormulario();
                await RecarregarGrid();

                MessageBox.Show(
                    "Operação realizada com sucesso!",
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
            if (bsListaEstoque.Current == null)
            {
                MessageBox.Show(
                    "Selecione um produto para editar.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _produtoSelecionado = (Produto)bsListaEstoque.Current;

            txtNomeProduto.Text = _produtoSelecionado.Nome;
            txtDescricaoProduto.Text = _produtoSelecionado.Descricao;
            txtValorUnitario.Text = _produtoSelecionado.Preco
                .ToString("N2", new CultureInfo("pt-BR"));

            nupEstoque.Value = _produtoSelecionado.Estoque;
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

        private void AtualizarProdutoSelecionado()
        {
            decimal novoPreco = decimal.Parse(txtValorUnitario.Text);
            int novoEstoque = (int)nupEstoque.Value;

            if (_produtoSelecionado.Preco != novoPreco)
                _produtoSelecionado.AtualizarPreco(novoPreco);

            int diferenca = novoEstoque - _produtoSelecionado.Estoque;

            if (diferenca > 0)
                _produtoSelecionado.AdicionarEstoque(diferenca);
            else if (diferenca < 0)
                _produtoSelecionado.RemoverEstoque(Math.Abs(diferenca));
        }

        private void LimparFormulario()
        {
            txtNomeProduto.Text = "";
            txtDescricaoProduto.Text = "";
            txtValorUnitario.Text = "";
            nupEstoque.Value = 0;

            _produtoSelecionado = null;
        }
        private async Task RecarregarGrid()
        {
            var produtos = await _produtoService.ObterTodos();
            bsListaEstoque.DataSource = produtos;
            bsListaEstoque.ResetBindings(false);
        }
    }
}
