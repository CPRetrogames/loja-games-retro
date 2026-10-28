using LojaGamesRetro.Data;
using LojaGamesRetro.Models;
using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Relatório de vendas: listagem dos pedidos registrados.
/// Filtros/indicadores e detalhes/exportação ficam nos arquivos parciais.
/// </summary>
public partial class RelatorioVendasForm : Form
{
    private readonly DataGridView gridVendas;
    private List<Venda> vendasExibidas = new();

    public RelatorioVendasForm()
    {
        Tema.AplicarForm(this, "Retro Games Store - Relatório de Vendas");
        ClientSize = new Size(1150, 720);
        MinimumSize = new Size(950, 600);

        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Tema.Painel };
        var lblTitulo = Tema.CriarLabel("RELATÓRIO DE VENDAS", Tema.Titulo(18), Tema.Amarelo);
        lblTitulo.Location = new Point(16, 16);
        topo.Controls.Add(lblTitulo);

        gridVendas = new DataGridView { Dock = DockStyle.Fill };
        Tema.EstilizarGrid(gridVendas);
        gridVendas.Columns.Add("Pedido", "PEDIDO");
        gridVendas.Columns.Add("Data", "DATA");
        gridVendas.Columns.Add("Cliente", "CLIENTE");
        gridVendas.Columns.Add("Itens", "ITENS");
        gridVendas.Columns.Add("Pagamento", "PAGAMENTO");
        gridVendas.Columns.Add("Total", "TOTAL");
        gridVendas.Columns["Pedido"].FillWeight = 60;
        gridVendas.Columns["Data"].FillWeight = 110;
        gridVendas.Columns["Cliente"].FillWeight = 160;
        gridVendas.Columns["Itens"].FillWeight = 50;
        gridVendas.Columns["Pagamento"].FillWeight = 110;
        gridVendas.Columns["Total"].FillWeight = 90;
        gridVendas.Columns["Itens"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        gridVendas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Ordem importa para o Dock: o último adicionado é posicionado primeiro.
        Controls.Add(gridVendas);
        ConfigurarDetalhes();   // painel inferior
        ConfigurarFiltros();    // filtros + indicadores (abaixo do título)
        Controls.Add(topo);

        CarregarVendas();
    }

    partial void ConfigurarFiltros();
    partial void ConfigurarDetalhes();
    partial void AplicarFiltros(ref IEnumerable<Venda> vendas);
    partial void AtualizarIndicadores();

    private void CarregarVendas()
    {
        IEnumerable<Venda> vendas = RepositorioVendas.Listar();
        AplicarFiltros(ref vendas);
        vendasExibidas = vendas.OrderByDescending(v => v.Data).ToList();

        gridVendas.Rows.Clear();
        foreach (var venda in vendasExibidas)
        {
            var indice = gridVendas.Rows.Add(
                venda.Codigo,
                venda.Data.ToString("dd/MM/yyyy HH:mm"),
                venda.Cliente,
                venda.QuantidadeItens,
                venda.FormaPagamento,
                venda.Total.ToString("C"));
            gridVendas.Rows[indice].Tag = venda;
        }

        AtualizarIndicadores();
    }
}
