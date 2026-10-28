using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Tela de abertura: mostra o logo da loja e uma barra de carregamento em blocos (estilo 8-bit).
/// </summary>
public class SplashForm : Form
{
    private readonly Panel barra;
    private readonly Label lblStatus;
    private readonly Label lblPorcentagem;
    private readonly Label lblInsertCoin;
    private readonly System.Windows.Forms.Timer timer;
    private int progresso;
    private int ticks;

    private readonly string[] mensagens =
    {
        "Soprando os cartuchos...",
        "Conectando os controles...",
        "Ajustando a TV de tubo...",
        "Carregando o catálogo...",
        "Pronto para jogar!"
    };

    public SplashForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(600, 340);
        BackColor = Tema.Fundo;
        ShowInTaskbar = false;
        DoubleBuffered = true;

        var lblLogo = new Label
        {
            Text = "RETRO GAMES",
            Font = Tema.Titulo(36),
            ForeColor = Tema.Neon,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(10, 50, 580, 70)
        };

        var lblSubtitulo = new Label
        {
            Text = "L O J A   D E   G A M E S   R E T R Ô",
            Font = Tema.Titulo(12),
            ForeColor = Tema.Rosa,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(10, 120, 580, 30)
        };

        lblInsertCoin = new Label
        {
            Text = "INSERT COIN",
            Font = Tema.Titulo(11),
            ForeColor = Tema.Amarelo,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(10, 165, 580, 24)
        };

        barra = new Panel
        {
            Bounds = new Rectangle(70, 220, 460, 26),
            BackColor = Tema.Painel
        };
        barra.Paint += DesenharBarra;

        lblPorcentagem = new Label
        {
            Text = "0%",
            Font = Tema.Titulo(10),
            ForeColor = Tema.Neon,
            TextAlign = ContentAlignment.MiddleRight,
            Bounds = new Rectangle(430, 252, 100, 22)
        };

        lblStatus = new Label
        {
            Text = mensagens[0],
            Font = Tema.Corpo(9),
            ForeColor = Tema.TextoSuave,
            Bounds = new Rectangle(70, 252, 360, 22)
        };

        var lblRodape = new Label
        {
            Text = "© 1985–2026 RETRO GAMES STORE",
            Font = Tema.Titulo(8),
            ForeColor = Tema.TextoSuave,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(10, 300, 580, 20)
        };

        Controls.AddRange(new Control[]
        {
            lblLogo, lblSubtitulo, lblInsertCoin, barra, lblPorcentagem, lblStatus, lblRodape
        });

        timer = new System.Windows.Forms.Timer { Interval = 35 };
        timer.Tick += AvancarCarregamento;
        Load += (_, _) => timer.Start();
    }

    private void AvancarCarregamento(object sender, EventArgs e)
    {
        ticks++;
        progresso = Math.Min(100, progresso + 1);

        var indice = Math.Min(progresso * mensagens.Length / 100, mensagens.Length - 1);
        lblStatus.Text = mensagens[indice];
        lblPorcentagem.Text = $"{progresso}%";

        // "INSERT COIN" piscando
        if (ticks % 12 == 0)
            lblInsertCoin.Visible = !lblInsertCoin.Visible;

        barra.Invalidate();

        if (progresso >= 100)
        {
            timer.Stop();
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void DesenharBarra(object sender, PaintEventArgs e)
    {
        const int blocos = 23;
        const int espaco = 3;
        var larguraBloco = (barra.Width - espaco * (blocos + 1)) / blocos;
        var preenchidos = progresso * blocos / 100;

        using var pincel = new SolidBrush(Tema.Neon);
        for (var i = 0; i < preenchidos; i++)
        {
            var x = espaco + i * (larguraBloco + espaco);
            e.Graphics.FillRectangle(pincel, x, 4, larguraBloco, barra.Height - 8);
        }

        Tema.DesenharBordaNeon(e.Graphics, barra.ClientRectangle, Tema.PainelClaro);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Tema.DesenharBordaNeon(e.Graphics, ClientRectangle, Tema.Neon, 3);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) timer.Dispose();
        base.Dispose(disposing);
    }
}
