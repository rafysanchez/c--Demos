using System;
using System.Collections.Generic;
using System.Linq;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    public override string ToString() =>
        $"{Id} - {Nome}: {Preco:C} (estoque: {Estoque})";
}

public class Program
{
    public static void Main()
    {
        Produto[] produtosArray =
        {
            new Produto { Id = 1, Nome = "Notebook", Preco = 3500m, Estoque = 5 },
            new Produto { Id = 2, Nome = "Mouse", Preco = 80m, Estoque = 20 },
            new Produto { Id = 3, Nome = "Teclado", Preco = 150m, Estoque = 0 },
            new Produto { Id = 4, Nome = "Monitor", Preco = 1200m, Estoque = 8 }
        };

        Console.WriteLine("ARRAY");
        Console.WriteLine("Percorrendo com for:");
        for (int i = 0; i < produtosArray.Length; i++)
            Console.WriteLine(produtosArray[i]);

        Console.WriteLine($"\nPrimeiro item do array: {produtosArray[0].Nome}");
        Produto? produtoPorId = produtosArray.FirstOrDefault(p => p.Id == 2);
        Console.WriteLine($"Busca por Id com LINQ: {produtoPorId}");

        produtosArray[1].Preco = 90m;

        Produto[] disponiveis = produtosArray.Where(p => p.Estoque > 0).ToArray();
        Produto[] maisCaros = produtosArray.OrderByDescending(p => p.Preco).ToArray();
        string[] nomes = produtosArray.Select(p => p.Nome).ToArray();

        Console.WriteLine("\nProdutos disponiveis:");
        foreach (Produto produto in disponiveis)
            Console.WriteLine(produto);

        Console.WriteLine($"\nMais caro: {maisCaros[0]}");
        Console.WriteLine("Nomes: " + string.Join(", ", nomes));
        Console.WriteLine($"Quantidade no array: {produtosArray.Length}");
        Console.WriteLine($"Preco medio: {produtosArray.Average(p => p.Preco):C}");
        Console.WriteLine($"Valor total em estoque: {produtosArray.Sum(p => p.Preco * p.Estoque):C}");
        Console.WriteLine($"Existe item sem estoque? {produtosArray.Any(p => p.Estoque == 0)}");

        Console.WriteLine("\nLIST");
        List<Produto> produtos = produtosArray.ToList();

        produtos.Add(new Produto { Id = 5, Nome = "Headset", Preco = 250m, Estoque = 12 });
        produtos.Insert(1, new Produto { Id = 6, Nome = "Webcam", Preco = 300m, Estoque = 4 });

        Produto? teclado = produtos.Find(p => p.Nome == "Teclado");
        bool temMonitor = produtos.Exists(p => p.Nome == "Monitor");
        int indiceMouse = produtos.FindIndex(p => p.Nome == "Mouse");

        Console.WriteLine($"Produto localizado com Find: {teclado}");
        Console.WriteLine($"Existe monitor? {temMonitor}");
        Console.WriteLine($"Indice do Mouse na lista: {indiceMouse}");

        if (indiceMouse >= 0)
            produtos[indiceMouse].Estoque += 5;

        produtos.RemoveAll(p => p.Estoque == 0);

        List<Produto> ordenadosPorNome = produtos.OrderBy(p => p.Nome).ToList();
        List<Produto> topPrecos = produtos
            .Where(p => p.Preco >= 250m)
            .OrderByDescending(p => p.Preco)
            .ToList();

        List<string> nomesFormatados = produtos.ConvertAll(
            p => $"{p.Nome.ToUpperInvariant()} - {p.Preco:C}");

        Console.WriteLine("\nLista ordenada por nome:");
        foreach (Produto produto in ordenadosPorNome)
            Console.WriteLine(produto);

        Console.WriteLine("\nProdutos com preco maior ou igual a 250:");
        foreach (Produto produto in topPrecos)
            Console.WriteLine(produto);

        Console.WriteLine("\nConvertAll para strings:");
        foreach (string nomeFormatado in nomesFormatados)
            Console.WriteLine(nomeFormatado);

        Console.WriteLine("\nOUTRAS OPCOES COMUNS");

        Dictionary<int, Produto> produtosPorId = produtos.ToDictionary(p => p.Id);
        if (produtosPorId.TryGetValue(4, out Produto? monitor))
            Console.WriteLine($"Dictionary por Id -> {monitor}");

        HashSet<string> categoriasVisitadas = new(StringComparer.OrdinalIgnoreCase)
        {
            "Perifericos",
            "Monitores",
            "perifericos"
        };
        Console.WriteLine($"HashSet elimina duplicidade: {categoriasVisitadas.Count} categorias");

        Queue<Produto> filaReposicao = new(produtos.Where(p => p.Estoque < 10));
        if (filaReposicao.Count > 0)
            Console.WriteLine($"Primeiro da fila de reposicao: {filaReposicao.Dequeue()}");

        Stack<string> historicoOperacoes = new();
        historicoOperacoes.Push("Carga inicial");
        historicoOperacoes.Push("Atualizacao de estoque do Mouse");
        historicoOperacoes.Push("Remocao de itens sem estoque");
        Console.WriteLine($"Ultima operacao da pilha: {historicoOperacoes.Pop()}");

        string csvNomes = string.Join(";", produtos.Select(p => p.Nome));
        Console.WriteLine($"CSV simples para exportacao: {csvNomes}");
    }
}
