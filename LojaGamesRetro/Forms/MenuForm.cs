using LojaGamesRetro.UI;

namespace LojaGamesRetro.Forms;

/// <summary>
/// Menu principal: ponto de navegação entre a loja (cards de jogos) e o relatório de vendas.
/// </summary>
public class MenuForm : Form
{

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

        Controls.AddRange(new Control[]
        {
            lblLogo, lblSubtitulo, btnLoja, btnRelatorio, btnSair
        });
    }

    private void AbrirLoja()
    {
        MessageBox.Show("Loja de jogos em construção.", "Em breve",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void AbrirRelatorio()
    {
        MessageBox.Show("Relatório de vendas em construção.", "Em breve",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var area = new Rectangle(20, 20, ClientSize.Width - 40, ClientSize.Height - 40);
        Tema.DesenharBordaNeon(e.Graphics, area, Tema.PainelClaro, 2);
    }
}
