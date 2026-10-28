using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Menu principal: ponto de navegação entre a loja (cards de jogos) e o relatório de vendas.
/// </summary>
public class MenuForm : Form
{
    private readonly Label lblRelogio;
    private readonly System.Windows.Forms.Timer timerRelogio;

    public MenuForm()
    {
        Tema.AplicarForm(this, "Retro Games Store - Menu Principal");
        ClientSize = new Size(760, 520);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        DoubleBuffered = true;

        var lblLogo = new Label
        {
            Text = "RETRO GAMES STORE",
            Font = Tema.Titulo(30),
            ForeColor = Tema.Neon,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(30, 40, ClientSize.Width - 60, 60)
        };

        var lblSubtitulo = new Label
        {
            Text = "— MENU PRINCIPAL —",
            Font = Tema.Titulo(12),
            ForeColor = Tema.Rosa,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(30, 100, ClientSize.Width - 60, 28)
        };

        var btnLoja = Tema.CriarBotao("►  LOJA DE JOGOS", Tema.Neon, 340, 56);
        var btnRelatorio = Tema.CriarBotao("►  RELATÓRIO DE VENDAS", Tema.Amarelo, 340, 56);
        var btnSair = Tema.CriarBotao("►  SAIR", Tema.Rosa, 340, 56);

        var x = (ClientSize.Width - btnLoja.Width) / 2;
        btnLoja.Location = new Point(x, 170);
        btnRelatorio.Location = new Point(x, 245);
        btnSair.Location = new Point(x, 320);

        btnLoja.Click += (_, _) => AbrirLoja();
        btnRelatorio.Click += (_, _) => AbrirRelatorio();
        btnSair.Click += (_, _) => Close();

        var rodape = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 40,
            BackColor = Tema.Painel
        };

        var lblCreditos = new Label
        {
            Text = "FIAP • C# Development • 3ESPS",
            Font = Tema.Titulo(9),
            ForeColor = Tema.TextoSuave,
            AutoSize = true,
            Location = new Point(16, 12)
        };

        lblRelogio = new Label
        {
            Font = Tema.Titulo(10),
            ForeColor = Tema.Neon,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleRight,
            Bounds = new Rectangle(ClientSize.Width - 236, 8, 220, 24)
        };

        rodape.Controls.Add(lblCreditos);
        rodape.Controls.Add(lblRelogio);

        Controls.AddRange(new Control[]
        {
            lblLogo, lblSubtitulo, btnLoja, btnRelatorio, btnSair, rodape
        });

        timerRelogio = new System.Windows.Forms.Timer { Interval = 1000 };
        timerRelogio.Tick += (_, _) => AtualizarRelogio();
        timerRelogio.Start();

        AtualizarRelogio();
    }

    private void AbrirLoja()
    {
        using var loja = new CardsJogosForm();
        Hide();
        loja.ShowDialog(this);
        Show();
    }

    private void AbrirRelatorio()
    {
        MessageBox.Show("Relatório de vendas em construção.", "Em breve",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void AtualizarRelogio()
    {
        lblRelogio.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var area = new Rectangle(20, 20, ClientSize.Width - 40, ClientSize.Height - 80);
        Tema.DesenharBordaNeon(e.Graphics, area, Tema.PainelClaro, 2);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) timerRelogio.Dispose();
        base.Dispose(disposing);
    }
}
