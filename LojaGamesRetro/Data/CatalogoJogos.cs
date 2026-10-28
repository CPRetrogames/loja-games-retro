using LojaGamesRetro.Models;

namespace LojaGamesRetro.Data;

/// <summary>
/// Catálogo em memória com os jogos disponíveis na loja.
/// </summary>
public static class CatalogoJogos
{
    public static List<Jogo> Jogos { get; } = new()
    {
        new() { Id = 1,  Titulo = "Super Mario World",       Plataforma = "SNES",        Ano = 1990, Genero = "Plataforma",  Preco = 249.90m, Estoque = 5 },
        new() { Id = 2,  Titulo = "Zelda: A Link to the Past", Plataforma = "SNES",      Ano = 1991, Genero = "Aventura",    Preco = 279.90m, Estoque = 3 },
        new() { Id = 3,  Titulo = "Donkey Kong Country",     Plataforma = "SNES",        Ano = 1994, Genero = "Plataforma",  Preco = 229.90m, Estoque = 4 },
        new() { Id = 4,  Titulo = "Sonic the Hedgehog 2",    Plataforma = "Mega Drive",  Ano = 1992, Genero = "Plataforma",  Preco = 189.90m, Estoque = 6 },
        new() { Id = 5,  Titulo = "Streets of Rage 2",       Plataforma = "Mega Drive",  Ano = 1992, Genero = "Beat 'em up", Preco = 199.90m, Estoque = 4 },
        new() { Id = 6,  Titulo = "Super Mario Bros. 3",     Plataforma = "NES",         Ano = 1988, Genero = "Plataforma",  Preco = 219.90m, Estoque = 3 },
        new() { Id = 7,  Titulo = "Contra",                  Plataforma = "NES",         Ano = 1988, Genero = "Ação",        Preco = 159.90m, Estoque = 5 },
        new() { Id = 8,  Titulo = "Pac-Man",                 Plataforma = "Atari 2600",  Ano = 1982, Genero = "Arcade",      Preco = 99.90m,  Estoque = 8 },
        new() { Id = 9,  Titulo = "Pokémon Red",             Plataforma = "Game Boy",    Ano = 1996, Genero = "RPG",         Preco = 299.90m, Estoque = 2 },
        new() { Id = 10, Titulo = "Tetris",                  Plataforma = "Game Boy",    Ano = 1989, Genero = "Puzzle",      Preco = 89.90m,  Estoque = 10 },
        new() { Id = 11, Titulo = "Crash Bandicoot",         Plataforma = "PlayStation", Ano = 1996, Genero = "Plataforma",  Preco = 179.90m, Estoque = 5 },
        new() { Id = 12, Titulo = "GoldenEye 007",           Plataforma = "Nintendo 64", Ano = 1997, Genero = "Tiro (FPS)",  Preco = 259.90m, Estoque = 3 },
    };

    public static IEnumerable<string> Plataformas =>
        Jogos.Select(j => j.Plataforma).Distinct().OrderBy(p => p);

    public static Jogo BuscarPorId(int id) => Jogos.FirstOrDefault(j => j.Id == id);
}
