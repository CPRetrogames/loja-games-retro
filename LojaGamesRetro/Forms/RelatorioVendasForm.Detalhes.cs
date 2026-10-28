using System.Text;
using LojaGamesRetro.Models;
using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Detalhe dos itens do pedido selecionado e exportação do relatório para CSV.
/// </summary>
public partial class RelatorioVendasForm
{
    private DataGridView gridItens;
    private Label lblDetalhe;

    partial void ConfigurarDetalhes()
    {
        var painelDetalhes = new Panel
        {
            Dock = DockStyle.Bottom,
            Width = ClientSize.Width,
            Height = 230,
            BackColor = Tema.Painel,
            Padding = new Padding(10, 46, 10, 10)
        };

        lblDetalhe = Tema.CriarLabel("ITENS DO PEDIDO", Tema.Titulo(12), Tema.Neon);
        lblDetalhe.Location = new Point(12, 14);

        var btnExportar = Tema.CriarBotao("EXPORTAR CSV", Tema.Amarelo, 160, 32);
        btnExportar.Font = Tema.Titulo(9);
        btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExportar.Location = new Point(ClientSize.Width - btnExportar.Width - 14, 8);
        btnExportar.Click += (_, _) => ExportarCsv();

        gridItens = new DataGridView { Dock = DockStyle.Fill };
        Tema.EstilizarGrid(gridItens);
        gridItens.Columns.Add("Jogo", "JOGO");
        gridItens.Columns.Add("Plataforma", "PLATAFORMA");
        gridItens.Columns.Add("Qtd", "QTD");
        gridItens.Columns.Add("Preco", "PREÇO UNIT.");
        gridItens.Columns.Add("Subtotal", "SUBTOTAL");
        gridItens.Columns["Jogo"].FillWeight = 200;
        gridItens.Columns["Qtd"].FillWeight = 50;
        gridItens.Columns["Qtd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        gridItens.Columns["Preco"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        gridItens.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        painelDetalhes.Controls.Add(gridItens);
        painelDetalhes.Controls.Add(lblDetalhe);
        painelDetalhes.Controls.Add(btnExportar);
        Controls.Add(painelDetalhes);

        gridVendas.SelectionChanged += (_, _) => ExibirItensDaVendaSelecionada();
    }

    private void ExibirItensDaVendaSelecionada()
    {
        gridItens.Rows.Clear();

        if (gridVendas.CurrentRow?.Tag is not Venda venda)
        {
            lblDetalhe.Text = "ITENS DO PEDIDO";
            return;
        }

        lblDetalhe.Text = $"ITENS DO PEDIDO {venda.Codigo} — {venda.Cliente}";
        foreach (var item in venda.Itens)
        {
            gridItens.Rows.Add(
                item.Jogo.Titulo,
                item.Jogo.Plataforma,
                item.Quantidade,
                item.PrecoUnitario.ToString("C"),
                item.Subtotal.ToString("C"));
        }
    }

    private void ExportarCsv()
    {
        if (vendasExibidas.Count == 0)
        {
            MessageBox.Show("Não há vendas para exportar com os filtros atuais.", "Exportar",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialogo = new SaveFileDialog
        {
            Filter = "Arquivo CSV (*.csv)|*.csv",
            FileName = $"relatorio-vendas-{DateTime.Now:yyyyMMdd-HHmm}.csv"
        };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        var csv = new StringBuilder();
        csv.AppendLine("Pedido;Data;Cliente;Pagamento;Jogo;Plataforma;Quantidade;Preco Unitario;Subtotal");
        foreach (var venda in vendasExibidas)
        {
            foreach (var item in venda.Itens)
            {
                csv.AppendLine(string.Join(";",
                    venda.Codigo,
                    venda.Data.ToString("dd/MM/yyyy HH:mm"),
                    venda.Cliente,
                    venda.FormaPagamento,
                    item.Jogo.Titulo,
                    item.Jogo.Plataforma,
                    item.Quantidade,
                    item.PrecoUnitario.ToString("N2"),
                    item.Subtotal.ToString("N2")));
            }
        }

        // UTF-8 com BOM para o Excel abrir os acentos corretamente
        File.WriteAllText(dialogo.FileName, csv.ToString(), new UTF8Encoding(true));
        MessageBox.Show("Relatório exportado com sucesso!", "Exportar",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
