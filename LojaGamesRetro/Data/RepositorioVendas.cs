using LojaGamesRetro.Models;

namespace LojaGamesRetro.Data;

/// <summary>
/// Armazena as vendas em memória enquanto o sistema está aberto.
/// </summary>
public static class RepositorioVendas
{
    public static readonly string[] FormasPagamento =
    {
        "PIX", "Cartão de Crédito", "Cartão de Débito", "Dinheiro"
    };

    private static readonly List<Venda> vendas = new();
    private static int proximoId = 1;

    static RepositorioVendas()
    {
        CarregarVendasDeExemplo();
    }

    public static IReadOnlyList<Venda> Listar() => vendas.AsReadOnly();

    public static Venda Registrar(string cliente, string formaPagamento, IEnumerable<ItemPedido> itens)
    {
        var listaItens = itens.ToList();

        if (string.IsNullOrWhiteSpace(cliente))
            throw new ArgumentException("Informe o nome do cliente.");
        if (listaItens.Count == 0)
            throw new ArgumentException("O pedido não possui itens.");

        foreach (var item in listaItens)
        {
            if (item.Quantidade > item.Jogo.Estoque)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para \"{item.Jogo.Titulo}\" (disponível: {item.Jogo.Estoque}).");
        }

        var venda = new Venda
        {
            Id = proximoId++,
            Data = DateTime.Now,
            Cliente = cliente.Trim(),
            FormaPagamento = formaPagamento,
            Itens = listaItens.Select(i => new ItemPedido
            {
                Jogo = i.Jogo,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

        foreach (var item in venda.Itens)
            item.Jogo.Estoque -= item.Quantidade;

        vendas.Add(venda);
        return venda;
    }

    private static void CarregarVendasDeExemplo()
    {
        AdicionarExemplo(DateTime.Today.AddDays(-6).AddHours(14), "Ana Souza", "PIX", (1, 1), (10, 2));
        AdicionarExemplo(DateTime.Today.AddDays(-3).AddHours(19), "Bruno Lima", "Cartão de Crédito", (4, 1), (5, 1));
        AdicionarExemplo(DateTime.Today.AddDays(-1).AddHours(11), "Carla Mendes", "Dinheiro", (8, 1));
    }

    private static void AdicionarExemplo(DateTime data, string cliente, string pagamento, params (int jogoId, int qtd)[] itens)
    {
        vendas.Add(new Venda
        {
            Id = proximoId++,
            Data = data,
            Cliente = cliente,
            FormaPagamento = pagamento,
            Itens = itens.Select(i =>
            {
                var jogo = CatalogoJogos.BuscarPorId(i.jogoId);
                return new ItemPedido { Jogo = jogo, Quantidade = i.qtd, PrecoUnitario = jogo.Preco };
            }).ToList()
        });
    }
}
