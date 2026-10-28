using System.Drawing.Drawing2D;
using LojaGamesRetro.Data;
using LojaGamesRetro.Models;
using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Vitrine da loja: exibe os jogos em cards, com filtro por plataforma e busca por título.
/// O carrinho e a finalização do pedido ficam em CardsJogosForm.Carrinho.cs.
/// </summary>
public partial class CardsJogosForm : Form
{
    private const string TodasPlataformas = "Todas";

    private readonly FlowLayoutPanel painelCards;
    private readonly Panel painelLateral;
    private readonly ComboBox cmbPlataforma;
    private readonly TextBox txtBusca;

    public CardsJogosForm()
    {
        Tema.AplicarForm(this, "Retro Games Store - Loja de Jogos");
        ClientSize = new Size(1200, 720);
        MinimumSize = new Size(1000, 600);

        // ---- Barra superior (título + filtros) ----
        var topo = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Tema.Painel };

        var lblTitulo = Tema.CriarLabel("CATÁLOGO DE JOGOS", Tema.Titulo(18), Tema.Neon);
        lblTitulo.Location = new Point(16, 20);

        var lblPlataforma = Tema.CriarLabel("Plataforma:", Tema.Corpo(10), Tema.TextoSuave);
        lblPlataforma.Location = new Point(360, 25);

        cmbPlataforma = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Bounds = new Rectangle(445, 21, 160, 28),
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            FlatStyle = FlatStyle.Flat
        };
        cmbPlataforma.Items.Add(TodasPlataformas);
        cmbPlataforma.Items.AddRange(CatalogoJogos.Plataformas.Cast<object>().ToArray());
        cmbPlataforma.SelectedIndex = 0;

        var lblBusca = Tema.CriarLabel("Buscar:", Tema.Corpo(10), Tema.TextoSuave);
        lblBusca.Location = new Point(625, 25);

        txtBusca = new TextBox
        {
            Bounds = new Rectangle(682, 22, 200, 28),
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            BorderStyle = BorderStyle.FixedSingle,
            PlaceholderText = "Título do jogo..."
        };

        topo.Controls.AddRange(new Control[] { lblTitulo, lblPlataforma, cmbPlataforma, lblBusca, txtBusca });

        // ---- Lateral (carrinho) ----
        painelLateral = new Panel
        {
            Dock = DockStyle.Right,
            Width = 370,
            BackColor = Tema.Painel,
            Padding = new Padding(14)
        };

        // ---- Área dos cards ----
        painelCards = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(12),
            BackColor = Tema.Fundo
        };

        // Ordem importa para o Dock: o último adicionado é posicionado primeiro.
        Controls.Add(painelCards);
        Controls.Add(painelLateral);
        Controls.Add(topo);

        cmbPlataforma.SelectedIndexChanged += (_, _) => RenderizarCards();
        txtBusca.TextChanged += (_, _) => RenderizarCards();

        ConfigurarCarrinho();
        RenderizarCards();
    }

    partial void ConfigurarCarrinho();

    partial void AdicionarAoCarrinho(Jogo jogo, int quantidade);

    private void RenderizarCards()
    {
        var plataforma = cmbPlataforma.SelectedItem as string ?? TodasPlataformas;
        var busca = txtBusca.Text.Trim();

        var jogos = CatalogoJogos.Jogos
            .Where(j => plataforma == TodasPlataformas || j.Plataforma == plataforma)
            .Where(j => busca.Length == 0 || j.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase))
            .ToList();

        painelCards.SuspendLayout();
        foreach (Control controle in painelCards.Controls.Cast<Control>().ToList())
            controle.Dispose();
        painelCards.Controls.Clear();

        if (jogos.Count == 0)
        {
            var vazio = Tema.CriarLabel("Nenhum jogo encontrado. GAME OVER :(", Tema.Titulo(12), Tema.Rosa);
            vazio.Margin = new Padding(20);
            painelCards.Controls.Add(vazio);
        }
        else
        {
            foreach (var jogo in jogos)
                painelCards.Controls.Add(CriarCard(jogo));
        }

        painelCards.ResumeLayout();
    }

    private Panel CriarCard(Jogo jogo)
    {
        var corPlataforma = Tema.CorPlataforma(jogo.Plataforma);

        var card = new Panel
        {
            Size = new Size(240, 340),
            BackColor = Tema.Painel,
            Margin = new Padding(10)
        };
        card.Paint += (_, e) => Tema.DesenharBordaNeon(e.Graphics, card.ClientRectangle, corPlataforma, 2);

        // "Capa" desenhada: gradiente na cor da plataforma + sigla do jogo
        var capa = new Panel { Bounds = new Rectangle(10, 10, 220, 125) };
        capa.Paint += (_, e) => DesenharCapa(e.Graphics, capa.ClientRectangle, jogo, corPlataforma);

        var lblTitulo = new Label
        {
            Text = jogo.Titulo,
            Font = Tema.Corpo(10.5f, FontStyle.Bold),
            ForeColor = Tema.Texto,
            Bounds = new Rectangle(10, 142, 220, 40),
            AutoEllipsis = true
        };

        var lblInfo = new Label
        {
            Text = $"{jogo.Plataforma} • {jogo.Ano} • {jogo.Genero}",
            Font = Tema.Corpo(8.5f),
            ForeColor = Tema.TextoSuave,
            Bounds = new Rectangle(10, 182, 220, 20)
        };

        var lblPreco = new Label
        {
            Text = jogo.Preco.ToString("C"),
            Font = Tema.Titulo(16),
            ForeColor = Tema.Amarelo,
            Bounds = new Rectangle(10, 206, 220, 32)
        };

        var lblEstoque = new Label
        {
            Text = jogo.Disponivel ? $"Em estoque: {jogo.Estoque}" : "ESGOTADO",
            Font = Tema.Titulo(9),
            ForeColor = jogo.Disponivel ? Tema.Neon : Tema.Rosa,
            Bounds = new Rectangle(10, 240, 220, 20)
        };

        var nudQuantidade = new NumericUpDown
        {
            Bounds = new Rectangle(10, 290, 60, 30),
            Minimum = 1,
            Maximum = Math.Max(1, jogo.Estoque),
            Value = 1,
            BackColor = Tema.PainelClaro,
            ForeColor = Tema.Texto,
            Font = Tema.Corpo(11),
            Enabled = jogo.Disponivel
        };

        var btnAdicionar = Tema.CriarBotao("+ CARRINHO", Tema.Neon, 150, 32);
        btnAdicionar.Font = Tema.Titulo(10);
        btnAdicionar.Location = new Point(80, 289);
        btnAdicionar.Enabled = jogo.Disponivel;
        btnAdicionar.Click += (_, _) => AdicionarAoCarrinho(jogo, (int)nudQuantidade.Value);

        card.Controls.AddRange(new Control[]
        {
            capa, lblTitulo, lblInfo, lblPreco, lblEstoque, nudQuantidade, btnAdicionar
        });

        return card;
    }

    private static void DesenharCapa(Graphics g, Rectangle area, Jogo jogo, Color cor)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        var escura = ControlPaint.Dark(cor, 0.4f);
        using (var gradiente = new LinearGradientBrush(area, cor, escura, LinearGradientMode.ForwardDiagonal))
            g.FillRectangle(gradiente, area);

        // "Scanlines" de TV de tubo
        using (var linha = new Pen(Color.FromArgb(35, 0, 0, 0)))
            for (var y = 0; y < area.Height; y += 4)
                g.DrawLine(linha, 0, y, area.Width, y);

        using var fontePlataforma = Tema.Titulo(8);
        using var fonteSigla = Tema.Titulo(34);
        using var branco = new SolidBrush(Color.White);
        using var sombra = new SolidBrush(Color.FromArgb(120, 0, 0, 0));

        g.DrawString(jogo.Plataforma.ToUpper(), fontePlataforma, branco, 8, 6);

        var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        var areaSombra = new RectangleF(area.X + 3, area.Y + 3, area.Width, area.Height);
        g.DrawString(jogo.Sigla, fonteSigla, sombra, areaSombra, formato);
        g.DrawString(jogo.Sigla, fonteSigla, branco, area, formato);
    }
}
