namespace CSharp14.Features;

// C# 14: Modificadores en parámetros lambda sin especificar tipo
internal class SimpleLambdaParameters
{
    public delegate bool TryParse<T>(string text, out T result);
    public delegate void ModifyValue(ref int value);
    public delegate void ProcessData(in ReadOnlySpan<byte> data);
    public delegate void ProcessScoped(scoped ReadOnlySpan<char> text);

    public static void Demo()
    {
        Console.WriteLine("=== C# 14: Simple Lambda Parameters with Modifiers ===");

        // C# 14: out sin tipo explícito
        TryParse<int> parse = (text, out result) => int.TryParse(text, out result);
        
        if (parse("123", out var number))
        {
            Console.WriteLine($"Parsed: {number}");
        }

        // ref sin tipo explícito
        ModifyValue doubleIt = (ref value) => value *= 2;
        int x = 5;
        doubleIt(ref x);
        Console.WriteLine($"After doubling: {x}");

        // in sin tipo explícito
        ProcessData processData = (in data) =>
        {
            Console.WriteLine($"Processing {data.Length} bytes");
        };
        ReadOnlySpan<byte> bytes = [1, 2, 3, 4];
        processData(in bytes);

        // scoped sin tipo explícito
        ProcessScoped processText = (scoped text) =>
        {
            Console.WriteLine($"Processing scoped text: {text.Length} chars");
        };
        ReadOnlySpan<char> chars = "Hello".AsSpan();
        processText(chars);

        // ref readonly sin tipo explícito
        Func<ReadOnlySpan<int>, int> sumSpan = (scoped span) =>
        {
            int sum = 0;
            foreach (var item in span)
                sum += item;
            return sum;
        };
        ReadOnlySpan<int> numbers = [1, 2, 3, 4, 5];
        Console.WriteLine($"Sum: {sumSpan(numbers)}");
    }
}
