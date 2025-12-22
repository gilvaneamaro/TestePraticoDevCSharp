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
    public partial class FormVenda : Form, INavigable, IClienteSelecionavel
    {
        private Cliente _clienteSelecionado;
        private IFormNavigator _navigator;
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;
        private Cliente _cliente;
        private Produto _produto;
        private List<ItemVenda> _itensVenda = new List<ItemVenda>();

        public FormVenda(ClienteService clienteService, ProdutoService produtoService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _produtoService = produtoService;
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

            if(quantidade > _produto.Estoque)
            {
                MessageBox.Show("Quantidade insuficiente em estoque.");
                return;
            }

            ItemVenda itemVenda = new ItemVenda(
                _produto.Id,
                _produto.Nome,
                quantidade,
                _produto.Preco
            );
            _produto.RemoverEstoque(quantidade);

            txtIDProduto.Clear();
            txtNomeProduto.Clear();
            txtDescricaoProduto.Clear();
            txtValorUnitario.Clear();
            txtQuantidadeEstoque.Clear();
            txtValorTotal.Clear();
            nupQuantidade.Value = 0;
            cbListaProdutos.SelectedIndex = -1;

            txtTotalPedido.Text = (_itensVenda.Sum(iv => iv.PrecoTotal) + itemVenda.PrecoTotal).ToString("N2");

            _itensVenda.Add(itemVenda);

            // Atualiza o DataGridView do carrinho
            dgvCarrinho.DataSource = null;
            dgvCarrinho.DataSource = _itensVenda; 
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {

        }

        private async void FormVenda_Load(object sender, EventArgs e)
        {
            var produtos = await _produtoService.ObterTodos();
            bsListaProdutos.DataSource = produtos;

            cbListaProdutos.DisplayMember = "Nome";
            cbListaProdutos.ValueMember = "Id";

            cbListaProdutos.SelectedIndex = -1;

        }

        private void cbListaProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbListaProdutos.SelectedItem is Produto produto)
            {
                _produto = produto;

                txtIDProduto.Text = produto.Id.ToString();
                txtNomeProduto.Text = produto.Nome;
                txtDescricaoProduto.Text = produto.Descricao;
                txtValorUnitario.Text = produto.Preco.ToString("N2");
                txtQuantidadeEstoque.Text = produto.Estoque.ToString();

                txtValorTotal.Text = (produto.Preco * nupQuantidade.Value).ToString("N2");
            }
            else
            {
                _produto = null;
            }
        }

        private void nupQuantidade_ValueChanged(object sender, EventArgs e)
        {
            txtValorTotal.Text = (_produto.Preco * (decimal)nupQuantidade.Value).ToString("N2");
        }
    }
}
