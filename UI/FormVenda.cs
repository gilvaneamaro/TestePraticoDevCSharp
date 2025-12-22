using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.App.Services;
using TestePraticoDevCSharp.Domain.Entities;
using TestePraticoDevCSharp.Domain.Enums;

namespace TestePraticoDevCSharp.UI
{
    public partial class FormVenda : Form, INavigable, IClienteSelecionavel
    {
        private IFormNavigator _navigator;
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;
        private readonly VendaService _vendaService;

        private Cliente _cliente;
        private Produto _produto;
        private List<ItemVenda> _itensVenda = new List<ItemVenda>();


        public FormVenda(ClienteService clienteService, ProdutoService produtoService, VendaService vendaService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _produtoService = produtoService;
            _vendaService = vendaService;
        }
        private async void FormVenda_Load(object sender, EventArgs e)
        {
            var produtos = await _produtoService.ObterTodos();

            bsListaProdutos.DataSource = produtos;
            cbListaProdutos.DisplayMember = "Nome";
            cbListaProdutos.ValueMember = "Id";
            cbListaProdutos.SelectedIndex = -1;
            cbMetodoPagamento.DataSource = Enum.GetValues(typeof(MetodoPagamento));
        }

        public void SetCliente(Cliente cliente)
        {
            _cliente = cliente;

            lblClienteNome.Text = "Nome: " + cliente.Nome;
            lblClienteTelefone.Text = "Telefone: " + cliente.Telefone;
            lblClienteEmail.Text = "Email: " + cliente.EmailEndereco;
        }
        public void SetNavigator(IFormNavigator navigator)
        {
            _navigator = navigator;
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            if (_produto == null)
            {
                MessageBox.Show("Selecione um produto antes de adicionar.");
                return;
            }

            int quantidade = (int)nupQuantidade.Value;

            if (quantidade <= 0)
            {
                MessageBox.Show("A quantidade deve ser maior que zero.");
                return;
            }

            if (!_produto.PodeVender(quantidade))
            {
                MessageBox.Show("Estoque insuficiente.");
                return;
            }

            ItemVenda itemVenda = new ItemVenda(
                _produto.Id,
                _produto.Nome,
                quantidade,
                _produto.Preco
            );
            _produto.RemoverEstoque(quantidade);

            LimparProdutoSelecionado();

            txtTotalPedido.Text = (_itensVenda.Sum(iv => iv.PrecoTotal) + itemVenda.PrecoTotal).ToString("N2");

            _itensVenda.Add(itemVenda);

            dgvCarrinho.DataSource = null;
            dgvCarrinho.DataSource = _itensVenda; 
        }

        private async void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (_itensVenda.Count == 0)
            {
                MessageBox.Show(
                    "Adicione ao menos um produto para finalizar a venda.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (_cliente == null)
            {
                MessageBox.Show("Selecione um cliente antes de finalizar a venda.");
                return;
            }

            var resultado = MessageBox.Show(
                "Confirma concluir a venda?",
                "Confirmar venda",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.No)
                return;

            try
            {
                var metodoPagamento = (MetodoPagamento)cbMetodoPagamento.SelectedItem;

                await _vendaService.FinalizarVendaAsync(
                    _cliente.Id,
                    metodoPagamento,
                    _itensVenda
                );

                MessageBox.Show(
                    "Venda finalizada com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                _itensVenda.Clear();
                dgvCarrinho.DataSource = null;
                txtTotalPedido.Clear();
                _navigator.Navigate<FormCaixa>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao finalizar a venda:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void cbListaProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var produto = cbListaProdutos.SelectedItem as Produto;

            if (produto == null)
            {
                LimparProdutoSelecionado();
                return;
            }

            _produto = produto;

            txtIDProduto.Text = produto.Id.ToString();
            txtNomeProduto.Text = produto.Nome;
            txtDescricaoProduto.Text = produto.Descricao;
            txtValorUnitario.Text = produto.Preco.ToString("N2");
            txtQuantidadeEstoque.Text = produto.Estoque.ToString();

            
        }

        private void nupQuantidade_ValueChanged(object sender, EventArgs e)
        {
            AtualizarValorTotalProduto();
        }

        private void LimparProdutoSelecionado()
        {
            _produto = null;

            txtIDProduto.Clear();
            txtNomeProduto.Clear();
            txtDescricaoProduto.Clear();
            txtValorUnitario.Clear();
            txtQuantidadeEstoque.Clear();
            txtValorTotal.Clear();

            nupQuantidade.Value = 0;
            cbListaProdutos.SelectedIndex = -1;
        }

        private void AtualizarValorTotalProduto()
        {
            if (_produto == null)
            {
                txtValorTotal.Text = "0,00";
                return;
            }

            txtValorTotal.Text =
                (_produto.Preco * nupQuantidade.Value).ToString("N2");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "Tem certeza que deseja cancelar a venda? Todos os itens serão removidos.",
                "Confirmar cancelamento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (resultado == DialogResult.No)
                return;
            _navigator.Navigate<FormCaixa>();
        }
    }
}
