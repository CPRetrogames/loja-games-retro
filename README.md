# 🕹️ Retro Games Store — Loja de Games Retrô

Sistema desktop em **C# Windows Forms (.NET 8)** para uma loja de games retrô, desenvolvido no **Checkpoint 3** da disciplina **C# Development** (Engenharia de Software — FIAP, turma 3ESPS).

## Telas

| Tela | Descrição |
|------|-----------|
| **Splash** | Abertura com logo da loja e barra de carregamento em blocos estilo 8-bit. |
| **Menu Principal** | Navegação para a loja e o relatório, resumo de pedidos/faturamento e relógio no rodapé. |
| **Cards de Jogos** | Vitrine em cards com filtro por plataforma e busca por título. Carrinho com quantidade, remoção de itens, nome do cliente, forma de pagamento e finalização do pedido (com baixa de estoque). |
| **Relatório de Vendas** | Listagem dos pedidos com filtros (período, cliente, pagamento), indicadores (pedidos, itens, faturamento, ticket médio, jogo mais vendido), itens do pedido selecionado e exportação para CSV. |

## Como executar

1. Abra `LojaGamesRetro.sln` no **Visual Studio 2022** (workload *Desenvolvimento para desktop com .NET*).
2. Pressione **F5**.

Ou pelo terminal (Windows):

```bash
dotnet run --project LojaGamesRetro
```

> Requer .NET 8 SDK. Os dados ficam em memória enquanto o sistema está aberto (o relatório já começa com algumas vendas de exemplo).

## Estrutura

```
LojaGamesRetro/
├── Data/      → CatalogoJogos (catálogo em memória) e RepositorioVendas
├── Forms/     → SplashForm, MenuForm, CardsJogosForm, RelatorioVendasForm
├── Models/    → Jogo, ItemPedido, Venda
├── UI/        → Tema (cores, fontes e componentes no visual retrô)
└── Program.cs → inicialização: Splash → Menu
```

## Fluxo de trabalho (Git)

- Branches principais: `main` e `dev`
- Uma branch por tarefa: `feat/splash`, `feat/menu`, `feat/cards-jogos`, `feat/relatorio-vendas`, `docs/entrega`
- Commits semânticos (`feat`, `fix`, `style`, `chore`, `docs`), no mínimo 3 por tarefa
- Pull Request de cada tarefa para `dev`; ao final, PR de `dev` para `main`

## Integrante

Veja o arquivo [`integrante.txt`](integrante.txt).
