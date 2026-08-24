using System;
using System.Collections.Generic;
using System.Linq;

// Enum e um value type que representa um conjunto fixo de valores nomeados.
enum StatusPedido
{
    Pendente = 1,
    Pago = 2,
    Enviado = 3
}

// Struct e um value type: ao copiar, os dados sao duplicados.
struct Ponto
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString() => $"({X}, {Y})";
}

// Record (classe) e um reference type util para dados imutaveis ou modelos simples.
record Produto(string Nome, decimal Preco);

// Class e um reference type: variaveis apontam para o mesmo objeto.
class Cliente
{
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }

    public override string ToString() => $"{Nome} ({Idade} anos)";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("TIPOS PRIMITIVOS");
        // Tipos numericos e logicos mais usados no dia a dia. Todos abaixo sao value types.
        bool ativo = true;
        char inicial = 'C';
        byte nivel = 10;
        short temperatura = -5;
        int idade = 30;
        long populacao = 8_000_000_000;
        float altura = 1.75f;
        double pi = 3.14159;
        decimal salario = 3500.99m;

        Console.WriteLine($"bool: {ativo}");
        Console.WriteLine($"char: {inicial}");
        Console.WriteLine($"byte: {nivel}");
        Console.WriteLine($"short: {temperatura}");
        Console.WriteLine($"int: {idade}");
        Console.WriteLine($"long: {populacao}");
        Console.WriteLine($"float: {altura}");
        Console.WriteLine($"double: {pi}");
        Console.WriteLine($"decimal: {salario}");

        Console.WriteLine("\nTEXTO, OBJECT E VAR");
        // string e reference type; object e reference type; var so infere o tipo automaticamente.
        string nome = "Maria";
        object qualquerCoisa = 123;
        var cidade = "Sao Paulo";

        Console.WriteLine($"string: {nome}");
        Console.WriteLine($"object: {qualquerCoisa}");
        Console.WriteLine($"var (inferido como string): {cidade}");

        Console.WriteLine("\nVALUE TYPES");
        // DateTime e struct sao value types. Alterar a copia nao altera o original.
        DateTime hoje = DateTime.Today;
        Ponto ponto1 = new() { X = 10, Y = 20 };
        Ponto ponto2 = ponto1;
        ponto2.X = 99;

        Console.WriteLine($"DateTime: {hoje:d}");
        Console.WriteLine($"struct original: {ponto1}");
        Console.WriteLine($"struct copiada e alterada: {ponto2}");

        Console.WriteLine("\nREFERENCE TYPES");
        // Cliente e class, portanto reference type. As variaveis compartilham a mesma referencia.
        Cliente cliente1 = new() { Nome = "Ana", Idade = 25 };
        Cliente cliente2 = cliente1;
        cliente2.Nome = "Ana Clara";

        Console.WriteLine($"cliente1: {cliente1}");
        Console.WriteLine($"cliente2: {cliente2}");

        Console.WriteLine("\nENUM");
        // Enum e value type e deixa o codigo mais legivel do que usar numeros soltos.
        StatusPedido status = StatusPedido.Pago;
        Console.WriteLine($"Enum: {status} = {(int)status}");

        Console.WriteLine("\nRECORD");
        // Este record e reference type porque foi declarado como record class implicitamente.
        // Ele facilita copia com alteracao usando "with".
        Produto produto1 = new("Notebook", 3500m);
        Produto produto2 = produto1 with { Preco = 3200m };

        Console.WriteLine($"record original: {produto1}");
        Console.WriteLine($"record com copia alterada: {produto2}");

        Console.WriteLine("\nNULLABLE");
        // int? e double? continuam sendo value types anulaveis; string? continua sendo reference type anulavel.
        int? estoque = null;
        double? desconto = 10.5;
        string? apelido = null;

        Console.WriteLine($"int?: {(estoque.HasValue ? estoque.Value : 0)}");
        Console.WriteLine($"double?: {desconto ?? 0}");
        Console.WriteLine($"string?: {apelido ?? "sem apelido"}");

        Console.WriteLine("\nCOLECOES COMUNS");
        // Array, List<T> e Dictionary<TKey, TValue> sao reference types.
        // Array tem tamanho fixo; List cresce dinamicamente; Dictionary busca por chave.
        int[] numeros = { 1, 2, 3 };
        List<string> tecnologias = new() { "C#", ".NET", "SQL" };
        Dictionary<int, string> usuarios = new()
        {
            [1] = "admin",
            [2] = "guest"
        };

        Console.WriteLine($"array: {string.Join(", ", numeros)}");
        Console.WriteLine($"List<T>: {string.Join(", ", tecnologias)}");
        Console.WriteLine($"Dictionary<TKey, TValue>: 1 -> {usuarios[1]}");

        Console.WriteLine("\nIENUMERABLE E IQUERYABLE");
        // IEnumerable<T> e uma interface de iteracao em memoria e e um reference type.
        // IQueryable<T> tambem e reference type e costuma ser usado com provedores como Entity Framework.
        IEnumerable<int> numerosParesEmMemoria = numeros.Where(n => n % 2 == 0);
        IQueryable<Produto> consultaProdutos =
            new List<Produto>
            {
                new("Mouse", 90m),
                new("Teclado", 150m),
                new("Monitor", 1200m)
            }
            .AsQueryable()
            .Where(p => p.Preco >= 100m);

        Console.WriteLine("IEnumerable<int>: " + string.Join(", ", numerosParesEmMemoria));
        Console.WriteLine("IQueryable<Produto>: " + string.Join(", ", consultaProdutos.Select(p => p.Nome)));
        Console.WriteLine("IEnumerable executa sobre dados em memoria; IQueryable pode traduzir a consulta para outra fonte.");

        Console.WriteLine("\nCONVERSOES");
        // Aqui convertemos de string (reference type) para int (value type).
        // Parse converte texto para numero; TryParse evita excecao em caso de erro.
        string numeroTexto = "42";
        int numero = int.Parse(numeroTexto);
        bool ok = int.TryParse("100", out int numeroSeguro);

        Console.WriteLine($"Parse: {numero}");
        Console.WriteLine($"TryParse: {ok} / valor = {numeroSeguro}");

        Console.WriteLine("\nBOXING E UNBOXING");
        // Boxing coloca um value type dentro de object (reference type); unboxing extrai de volta.
        int valor = 50;
        object caixa = valor;
        int valorDesempacotado = (int)caixa;

        Console.WriteLine($"boxing em object: {caixa}");
        Console.WriteLine($"unboxing para int: {valorDesempacotado}");

        Console.WriteLine("\nREF, OUT E IN");
        // ref altera a variavel original; out retorna valor; in passa somente leitura.
        // ref/out/in funcionam tanto com value types quanto com reference types.
        int a = 10;
        int b = 20;
        Trocar(ref a, ref b);
        Console.WriteLine($"ref -> a={a}, b={b}");

        // Aqui a variavel "soma" e declarada na propria chamada.
        // O escopo dela vai deste ponto ate o fim do bloco atual.
        // Se quiser reutilizar em varios pontos, voce pode declarar antes:
        // int soma;
        // Somar(5, 7, out soma);
        Somar(5, 7, out int soma);
        Console.WriteLine($"out -> soma={soma}");

        // Aqui o metodo retorna bool e tambem preenche o out.
        // Nesse caso, faz sentido guardar o retorno em outra variavel.
        var ret = TentarSomar(2, 3, out int somaCalculada);
        Console.WriteLine($"bool + out -> ret={ret}, somaCalculada={somaCalculada}");

        ExibirCliente(in cliente1);

        Console.WriteLine("\nTIPOS ANONIMOS E TUPLAS");
        // Tipo anonimo e reference type; tupla comum em C# e value type.
        // Tipo anonimo e util para objetos temporarios; tupla agrupa valores sem criar classe.
        var anonimo = new { Titulo = "Dev", Nivel = "Pleno" };
        (string Nome, int Pontos) ranking = ("Maria", 100);
        var coordenada = (X: 10, Y: 20);
        var retornoMetodo = ObterResumoPedido();
        var (produtoResumo, totalResumo, aprovadoResumo) = ObterResumoPedido();
        var (_, totalIgnorado, _) = ObterResumoPedido();

        Console.WriteLine($"anonimo: {anonimo.Titulo} - {anonimo.Nivel}");
        Console.WriteLine($"tupla: {ranking.Nome} - {ranking.Pontos}");
        Console.WriteLine($"tupla nomeada: X={coordenada.X}, Y={coordenada.Y}");
        Console.WriteLine(
            $"tupla retornada por metodo: {retornoMetodo.Produto} - {retornoMetodo.Total:C} - aprovado={retornoMetodo.Aprovado}");
        Console.WriteLine(
            $"desconstrucao de tupla: produto={produtoResumo}, total={totalResumo:C}, aprovado={aprovadoResumo}");
        Console.WriteLine($"descarte com _: total={totalIgnorado:C}");
    }

    static void Trocar(ref int x, ref int y)
    {
        // Como usa ref, a troca acontece nas variaveis originais.
        int temp = x;
        x = y;
        y = temp;
    }

    static void Somar(int x, int y, out int resultado)
    {
        // out obriga o metodo a atribuir valor antes de terminar.
        // Quem chama nao precisa inicializar a variavel antes.
        resultado = x + y;
    }

    static bool TentarSomar(int x, int y, out int resultado)
    {
        // Este metodo retorna dois resultados:
        // 1. o retorno bool, indicando sucesso
        // 2. o valor calculado via out
        resultado = x + y;
        return true;
    }

    static void ExibirCliente(in Cliente cliente)
    {
        // in evita copia desnecessaria e impede alteracao do parametro.
        Console.WriteLine($"in -> {cliente}");
    }

    static (string Produto, decimal Total, bool Aprovado) ObterResumoPedido()
    {
        // Tupla como retorno permite devolver varios valores sem criar uma classe so para isso.
        return ("Notebook", 3500m, true);
    }
}
