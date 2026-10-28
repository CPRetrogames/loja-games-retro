namespace LojaGamesRetro.Models;

public class ItemPedido
{
    public Jogo Jogo { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
