namespace LojaGamesRetro.Models;

public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Cliente { get; set; }
    public string FormaPagamento { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();

    public int QuantidadeItens => Itens.Sum(i => i.Quantidade);
    public decimal Total => Itens.Sum(i => i.Subtotal);
    public string Codigo => $"#{Id:D4}";
}
