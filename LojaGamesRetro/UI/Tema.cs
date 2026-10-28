using System.Drawing.Drawing2D;

namespace LojaGamesRetro.UI;

/// <summary>
/// Paleta de cores, fontes e componentes reutilizáveis com visual retrô (neon / arcade).
/// </summary>
public static class Tema
{
    public static readonly Color Fundo = Color.FromArgb(18, 10, 38);
    public static readonly Color Painel = Color.FromArgb(34, 22, 66);
    public static readonly Color PainelClaro = Color.FromArgb(52, 36, 96);
    public static readonly Color Neon = Color.FromArgb(0, 240, 200);
    public static readonly Color Rosa = Color.FromArgb(255, 64, 160);
    public static readonly Color Amarelo = Color.FromArgb(255, 204, 0);
    public static readonly Color Texto = Color.FromArgb(235, 235, 245);
    public static readonly Color TextoSuave = Color.FromArgb(170, 160, 200);

    public const string FonteRetro = "Consolas";

    public static Font Titulo(float tamanho) => new(FonteRetro, tamanho, FontStyle.Bold);

    public static Font Corpo(float tamanho = 10f, FontStyle estilo = FontStyle.Regular) =>
        new("Segoe UI", tamanho, estilo);

    public static void AplicarForm(Form form, string titulo)
    {
        form.Text = titulo;
        form.BackColor = Fundo;
        form.ForeColor = Texto;
        form.Font = Corpo();
        form.StartPosition = FormStartPosition.CenterScreen;
    }

    public static Label CriarLabel(string texto, Font fonte, Color cor) => new()
    {
        Text = texto,
        Font = fonte,
        ForeColor = cor,
        BackColor = Color.Transparent,
        AutoSize = true
    };

    public static Button CriarBotao(string texto, Color destaque, int largura = 220, int altura = 44)
    {
        var botao = new Button
        {
            Text = texto,
            Width = largura,
            Height = altura,
            FlatStyle = FlatStyle.Flat,
            BackColor = Painel,
            ForeColor = destaque,
            Font = Titulo(11),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        botao.FlatAppearance.BorderColor = destaque;
        botao.FlatAppearance.BorderSize = 2;

        return botao;
    }

    public static Color CorPlataforma(string plataforma) => plataforma switch
    {
        "SNES" => Color.FromArgb(110, 70, 200),
        "NES" => Color.FromArgb(200, 40, 50),
        "Mega Drive" => Color.FromArgb(30, 90, 200),
        "Atari 2600" => Color.FromArgb(210, 110, 20),
        "Game Boy" => Color.FromArgb(70, 140, 60),
        "PlayStation" => Color.FromArgb(70, 80, 110),
        "Nintendo 64" => Color.FromArgb(20, 140, 120),
        _ => PainelClaro
    };

    public static void DesenharBordaNeon(Graphics g, Rectangle area, Color cor, int espessura = 2)
    {
        g.SmoothingMode = SmoothingMode.None;
        using var caneta = new Pen(cor, espessura);
        g.DrawRectangle(caneta, area.X + espessura / 2, area.Y + espessura / 2,
            area.Width - espessura, area.Height - espessura);
    }

    public static void EstilizarGrid(DataGridView grid)
    {
        grid.BackgroundColor = Fundo;
        grid.BorderStyle = BorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersDefaultCellStyle.BackColor = PainelClaro;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Neon;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = PainelClaro;
        grid.ColumnHeadersDefaultCellStyle.Font = Titulo(10);
        grid.DefaultCellStyle.BackColor = Painel;
        grid.DefaultCellStyle.ForeColor = Texto;
        grid.DefaultCellStyle.SelectionBackColor = Rosa;
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.DefaultCellStyle.Font = Corpo(10);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(28, 18, 56);
        grid.GridColor = PainelClaro;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.ReadOnly = true;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowTemplate.Height = 30;
    }
}
