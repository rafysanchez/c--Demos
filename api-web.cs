using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/produtos", ProdutosEndpoints.ListarProdutos);
app.MapGet("/produtos/{id:int}", ProdutosEndpoints.BuscarProduto);
app.MapPost("/produtos", ProdutosEndpoints.CriarProduto);
app.MapPut("/produtos/{id:int}", ProdutosEndpoints.AtualizarProduto);
app.MapDelete("/produtos/{id:int}", ProdutosEndpoints.ExcluirProduto);

app.Run();

public static class ProdutosEndpoints
{
    private static readonly List<Produto> Produtos =
    [
        new(1, "Teclado", 120.00m, true),
        new(2, "Mouse", 80.00m, false),
        new(3, "Monitor", 900.00m, true)
    ];

    public static IResult ListarProdutos(string? busca = null)
    {
        var resultado = Produtos
            .Where(produto => string.IsNullOrWhiteSpace(busca) ||
                              produto.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase))
            .OrderBy(produto => produto.Nome)
            .Select(produto => new
            {
                produto.Id,
                produto.Nome,
                produto.Preco,
                produto.Ativo
            })
            .ToList();

        return Results.Ok(resultado);
    }

    public static IResult BuscarProduto(int id)
    {
        var produto = Produtos.FirstOrDefault(produto => produto.Id == id);
        return produto is null ? Results.NotFound() : Results.Ok(produto);
    }

    public static IResult CriarProduto(ProdutoRequest requisicao)
    {
        if (!Validar(requisicao, out var erro))
        {
            return Results.BadRequest(new { erro });
        }

        var id = Produtos.Count == 0 ? 1 : Produtos.Max(produto => produto.Id) + 1;
        var produto = new Produto(id, requisicao.Nome.Trim(), requisicao.Preco, requisicao.Ativo);
        Produtos.Add(produto);

        return Results.Created($"/produtos/{produto.Id}", produto);
    }

    public static IResult AtualizarProduto(int id, ProdutoRequest requisicao)
    {
        if (!Validar(requisicao, out var erro))
        {
            return Results.BadRequest(new { erro });
        }

        var indice = Produtos.FindIndex(produto => produto.Id == id);
        if (indice < 0)
        {
            return Results.NotFound();
        }

        var produtoAtualizado = new Produto(id, requisicao.Nome.Trim(), requisicao.Preco, requisicao.Ativo);
        Produtos[indice] = produtoAtualizado;

        return Results.Ok(produtoAtualizado);
    }

    public static IResult ExcluirProduto(int id)
    {
        var indice = Produtos.FindIndex(produto => produto.Id == id);
        if (indice < 0)
        {
            return Results.NotFound();
        }

        Produtos.RemoveAt(indice);
        return Results.NoContent();
    }

    private static bool Validar(ProdutoRequest requisicao, out string? erro)
    {
        if (string.IsNullOrWhiteSpace(requisicao.Nome))
        {
            erro = "O nome do produto é obrigatório.";
            return false;
        }

        if (requisicao.Preco < 0)
        {
            erro = "O preço do produto não pode ser negativo.";
            return false;
        }

        erro = null;
        return true;
    }
}

public record Produto(int Id, string Nome, decimal Preco, bool Ativo);
public record ProdutoRequest(string Nome, decimal Preco, bool Ativo);
