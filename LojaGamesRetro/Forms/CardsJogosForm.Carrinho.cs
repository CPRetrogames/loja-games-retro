using LojaGamesRetro.Data;
using LojaGamesRetro.Models;
using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Carrinho de compras e lançamento do pedido (parte da tela de cards).
/// </summary>
public partial class CardsJogosForm
{
    private readonly List<ItemPedido> carrinho = new();

    private ListView lvCarrinho;
    private TextBox txtCliente;
    private ComboBox cmbPagamento;
    private Label lblTotal;
    private Button btnFinalizar;

    partial void ConfigurarCarrinho()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            BackColor = Color.Transparent
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var lblTitulo = Tema.CriarLabel("SEU PEDIDO", Tema.Titulo(16), Tema.Neon);
        lblTitulo.Margin = new Padding(0, 0, 0, 10);

        lvCarrinho = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            BackColor = Tema.Fundo,
            ForeColor = Tema.Texto,
            BorderStyle = BorderStyle.FixedSingle,
            Font = Tema.Corpo(9.5f)
        };
        lvCarrinho.Columns.Add("Jogo", 165);
        lvCarrinho.Columns.Add("Qtd", 40, HorizontalAlignment.Center);
        lvCarrinho.Columns.Add("Subtotal", 100, HorizontalAlignment.Right);

        var botoesCarrinho = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 8, 0, 8),
            BackColor = Color.Transparent
        };
        var btnRemover = Tema.CriarBotao("REMOVER ITEM", Tema.Rosa, 160, 32);
        btnRemover.Font = Tema.Titulo(9);
        var btnLimpar = Tema.CriarBotao("LIMPAR", Tema.TextoSuave, 160, 32);
        btnLimpar.Font = Tema.Titulo(9);
        btnRemover.Click += (_, _) => RemoverItemSelecionado();
        btnLimpar.Click += (_, _) => LimparCarrinho();
        botoesCarrinho.Controls.AddRange(new Control[] { btnRemover, btnLimpar });

        var lblCliente = Tema.CriarLabel("Nome do cliente", Tema.Corpo(9.5f), Tema.TextoSuave);
        txtCliente = new TextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            BorderStyle = BorderStyle.FixedSingle,
            Font = Tema.Corpo(11),
            Margin = new Padding(0, 2, 0, 8)
        };

        var lblPagamento = Tema.CriarLabel("Forma de pagamento", Tema.Corpo(9.5f), Tema.TextoSuave);
        cmbPagamento = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            FlatStyle = FlatStyle.Flat,
            Font = Tema.Corpo(11),
            Margin = new Padding(0, 2, 0, 8)
        };
        cmbPagamento.Items.AddRange(RepositorioVendas.FormasPagamento.Cast<object>().ToArray());
        cmbPagamento.SelectedIndex = 0;

        lblTotal = new Label
        {
            Dock = DockStyle.Fill,
            Height = 44,
            Font = Tema.Titulo(18),
            ForeColor = Tema.Amarelo,
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(0, 6, 0, 6)
        };

        btnFinalizar = Tema.CriarBotao("FINALIZAR PEDIDO", Tema.Amarelo, 330, 48);
        btnFinalizar.Dock = DockStyle.Fill;
        btnFinalizar.Click += (_, _) => FinalizarPedido();

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // título
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // lista
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // botões
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // label cliente
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // cliente
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // label pagamento
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // pagamento
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));   // total
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));   // finalizar

        layout.Controls.Add(lblTitulo, 0, 0);
        layout.Controls.Add(lvCarrinho, 0, 1);
        layout.Controls.Add(botoesCarrinho, 0, 2);
        layout.Controls.Add(lblCliente, 0, 3);
        layout.Controls.Add(txtCliente, 0, 4);
        layout.Controls.Add(lblPagamento, 0, 5);
        layout.Controls.Add(cmbPagamento, 0, 6);
        layout.Controls.Add(lblTotal, 0, 7);
        layout.Controls.Add(btnFinalizar, 0, 8);

        painelLateral.Controls.Add(layout);
        AtualizarCarrinho();
    }

    partial void AdicionarAoCarrinho(Jogo jogo, int quantidade)
    {
        var item = carrinho.FirstOrDefault(i => i.Jogo.Id == jogo.Id);
        var jaNoCarrinho = item?.Quantidade ?? 0;

        if (jaNoCarrinho + quantidade > jogo.Estoque)
        {
            MessageBox.Show(
                $"Estoque insuficiente para \"{jogo.Titulo}\".\nDisponível para adicionar: {jogo.Estoque - jaNoCarrinho}.",
                "Estoque", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (item == null)
            carrinho.Add(new ItemPedido { Jogo = jogo, Quantidade = quantidade, PrecoUnitario = jogo.Preco });
        else
            item.Quantidade += quantidade;

        AtualizarCarrinho();
    }

    private void AtualizarCarrinho()
    {
        lvCarrinho.BeginUpdate();
        lvCarrinho.Items.Clear();
        foreach (var item in carrinho)
        {
            var linha = new ListViewItem(item.Jogo.Titulo) { Tag = item };
            linha.SubItems.Add(item.Quantidade.ToString());
            linha.SubItems.Add(item.Subtotal.ToString("C"));
            lvCarrinho.Items.Add(linha);
        }
        lvCarrinho.EndUpdate();

        var total = carrinho.Sum(i => i.Subtotal);
        lblTotal.Text = $"TOTAL {total:C}";
        btnFinalizar.Enabled = carrinho.Count > 0;
    }

    private void RemoverItemSelecionado()
    {
        if (lvCarrinho.SelectedItems.Count == 0)
        {
            MessageBox.Show("Selecione um item do carrinho para remover.", "Carrinho",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        carrinho.Remove((ItemPedido)lvCarrinho.SelectedItems[0].Tag);
        AtualizarCarrinho();
    }

    private void LimparCarrinho()
    {
        if (carrinho.Count == 0) return;

        var confirmar = MessageBox.Show("Deseja esvaziar o carrinho?", "Carrinho",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes) return;

        carrinho.Clear();
        AtualizarCarrinho();
    }

    private void FinalizarPedido()
    {
        if (carrinho.Count == 0) return;

        if (string.IsNullOrWhiteSpace(txtCliente.Text))
        {
            MessageBox.Show("Informe o nome do cliente para finalizar o pedido.", "Pedido",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCliente.Focus();
            return;
        }

        var total = carrinho.Sum(i => i.Subtotal);
        var quantidade = carrinho.Sum(i => i.Quantidade);
        var confirmar = MessageBox.Show(
            $"Cliente: {txtCliente.Text.Trim()}\nItens: {quantidade}\nPagamento: {cmbPagamento.SelectedItem}\nTotal: {total:C}\n\nConfirmar pedido?",
            "Confirmar pedido", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes) return;

        try
        {
            var venda = RepositorioVendas.Registrar(txtCliente.Text, cmbPagamento.SelectedItem.ToString(), carrinho);

            MessageBox.Show($"Pedido {venda.Codigo} registrado com sucesso!\nTotal: {venda.Total:C}",
                "PLAYER 1 WINS!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            carrinho.Clear();
            txtCliente.Clear();
            cmbPagamento.SelectedIndex = 0;
            AtualizarCarrinho();
            RenderizarCards();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            MessageBox.Show(ex.Message, "Não foi possível finalizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
