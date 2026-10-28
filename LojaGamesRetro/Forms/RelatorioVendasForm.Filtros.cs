using LojaGamesRetro.Data;
using LojaGamesRetro.Models;
using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Filtros (período, cliente, pagamento) e indicadores do relatório de vendas.
/// </summary>
public partial class RelatorioVendasForm
{
    private const string TodosPagamentos = "Todos";

    private DateTimePicker dtInicio;
    private DateTimePicker dtFim;
    private TextBox txtFiltroCliente;
    private ComboBox cmbFiltroPagamento;

    private Label lblPedidos;
    private Label lblItens;
    private Label lblFaturamento;
    private Label lblTicketMedio;
    private Label lblMaisVendido;

    partial void ConfigurarFiltros()
    {
        // ---- Indicadores ----
        var painelIndicadores = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 92,
            Padding = new Padding(10, 8, 10, 8),
            BackColor = Tema.Fundo,
            WrapContents = false
        };
        lblPedidos = CriarIndicador(painelIndicadores, "PEDIDOS", Tema.Neon, 130);
        lblItens = CriarIndicador(painelIndicadores, "ITENS VENDIDOS", Tema.Neon, 150);
        lblFaturamento = CriarIndicador(painelIndicadores, "FATURAMENTO", Tema.Amarelo, 200);
        lblTicketMedio = CriarIndicador(painelIndicadores, "TICKET MÉDIO", Tema.Amarelo, 170);
        lblMaisVendido = CriarIndicador(painelIndicadores, "MAIS VENDIDO", Tema.Rosa, 330);

        // ---- Filtros ----
        var painelFiltros = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(10, 10, 10, 6),
            BackColor = Tema.Painel,
            WrapContents = false
        };

        dtInicio = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 115, Value = DateTime.Today.AddDays(-30) };
        dtFim = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 115, Value = DateTime.Today };

        txtFiltroCliente = new TextBox
        {
            Width = 170,
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            BorderStyle = BorderStyle.FixedSingle,
            PlaceholderText = "Nome do cliente"
        };

        cmbFiltroPagamento = new ComboBox
        {
            Width = 150,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            FlatStyle = FlatStyle.Flat
        };
        cmbFiltroPagamento.Items.Add(TodosPagamentos);
        cmbFiltroPagamento.Items.AddRange(RepositorioVendas.FormasPagamento.Cast<object>().ToArray());
        cmbFiltroPagamento.SelectedIndex = 0;

        var btnFiltrar = Tema.CriarBotao("FILTRAR", Tema.Neon, 100, 28);
        btnFiltrar.Font = Tema.Titulo(9);
        btnFiltrar.Click += (_, _) => CarregarVendas();

        var btnLimpar = Tema.CriarBotao("LIMPAR", Tema.TextoSuave, 90, 28);
        btnLimpar.Font = Tema.Titulo(9);
        btnLimpar.Click += (_, _) => LimparFiltros();

        painelFiltros.Controls.AddRange(new Control[]
        {
            CriarRotulo("De:"), dtInicio,
            CriarRotulo("Até:"), dtFim,
            CriarRotulo("Cliente:"), txtFiltroCliente,
            CriarRotulo("Pagamento:"), cmbFiltroPagamento,
            btnFiltrar, btnLimpar
        });

        Controls.Add(painelIndicadores);
        Controls.Add(painelFiltros);
    }

    partial void AplicarFiltros(ref IEnumerable<Venda> vendas)
    {
        var inicio = dtInicio.Value.Date;
        var fim = dtFim.Value.Date.AddDays(1);
        var cliente = txtFiltroCliente.Text.Trim();
        var pagamento = cmbFiltroPagamento.SelectedItem as string ?? TodosPagamentos;

        vendas = vendas
            .Where(v => v.Data >= inicio && v.Data < fim)
            .Where(v => cliente.Length == 0 || v.Cliente.Contains(cliente, StringComparison.OrdinalIgnoreCase))
            .Where(v => pagamento == TodosPagamentos || v.FormaPagamento == pagamento);
    }

    partial void AtualizarIndicadores()
    {
        var pedidos = vendasExibidas.Count;
        var itens = vendasExibidas.Sum(v => v.QuantidadeItens);
        var faturamento = vendasExibidas.Sum(v => v.Total);
        var ticketMedio = pedidos > 0 ? faturamento / pedidos : 0m;

        var maisVendido = vendasExibidas
            .SelectMany(v => v.Itens)
            .GroupBy(i => i.Jogo.Titulo)
            .Select(g => new { Titulo = g.Key, Quantidade = g.Sum(i => i.Quantidade) })
            .OrderByDescending(x => x.Quantidade)
            .FirstOrDefault();

        lblPedidos.Text = pedidos.ToString();
        lblItens.Text = itens.ToString();
        lblFaturamento.Text = faturamento.ToString("C");
        lblTicketMedio.Text = ticketMedio.ToString("C");
        lblMaisVendido.Text = maisVendido == null ? "—" : $"{maisVendido.Titulo} ({maisVendido.Quantidade})";
    }

    private void LimparFiltros()
    {
        dtInicio.Value = DateTime.Today.AddDays(-30);
        dtFim.Value = DateTime.Today;
        txtFiltroCliente.Clear();
        cmbFiltroPagamento.SelectedIndex = 0;
        CarregarVendas();
    }

    private static Label CriarRotulo(string texto) => new()
    {
        Text = texto,
        AutoSize = true,
        ForeColor = Tema.TextoSuave,
        Margin = new Padding(8, 6, 2, 0)
    };

    private static Label CriarIndicador(FlowLayoutPanel destino, string titulo, Color cor, int largura)
    {
        var caixa = new Panel { Size = new Size(largura, 72), BackColor = Tema.Painel, Margin = new Padding(6, 0, 6, 0) };
        caixa.Paint += (_, e) => Tema.DesenharBordaNeon(e.Graphics, caixa.ClientRectangle, cor, 1);

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = Tema.Titulo(8),
            ForeColor = Tema.TextoSuave,
            Bounds = new Rectangle(10, 8, largura - 20, 16)
        };
        var lblValor = new Label
        {
            Text = "—",
            Font = Tema.Titulo(14),
            ForeColor = cor,
            Bounds = new Rectangle(10, 28, largura - 20, 34),
            AutoEllipsis = true
        };

        caixa.Controls.Add(lblTitulo);
        caixa.Controls.Add(lblValor);
        destino.Controls.Add(caixa);
        return lblValor;
    }
}
