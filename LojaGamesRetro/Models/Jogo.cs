namespace LojaGamesRetro.Models;

public class Jogo
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Plataforma { get; set; }
    public int Ano { get; set; }
    public string Genero { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    public bool Disponivel => Estoque > 0;

    /// <summary>Sigla usada na "capa" do card (ex.: "Super Mario World" → "SMW").</summary>
    public string Sigla =>
        string.Concat(Titulo
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(p => char.IsLetterOrDigit(p[0]))
            .Take(3)
            .Select(p => char.ToUpper(p[0])));
}
