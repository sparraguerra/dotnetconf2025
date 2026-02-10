namespace CSharp13.Features;
// C# 13: Modificadores requieren tipos explícitos en lambdas

internal class SimpleLambdaParameters
{
    public delegate bool TryParse<T>(string text, out T result);
    public delegate void ModifyValue(ref int value);
    public delegate void ProcessData(in ReadOnlySpan<byte> data);
    public delegate void ProcessScoped(scoped ReadOnlySpan<char> text);
    public delegate int SumSpanDelegate(ReadOnlySpan<int> span);

    public static void Demo()
    {
        Console.WriteLine("=== C# 13: Lambda Parameters with Modifiers Require Explicit Types ===");

        // C# 13: out requiere tipo explícito
        TryParse<int> parse = (string text, out int result) => int.TryParse(text, out result);

        if (parse("123", out var number))
        {
            Console.WriteLine($"Parsed: {number}");
        }

        // ref requiere tipo explícito
        ModifyValue doubleIt = (ref int value) => value *= 2;
        int x = 5;
        doubleIt(ref x);
        Console.WriteLine($"After doubling: {x}");

        // in requiere tipo explícito
        ProcessData processData = (in ReadOnlySpan<byte> data) =>
        {
            Console.WriteLine($"Processing {data.Length} bytes");
        };
        ReadOnlySpan<byte> bytes = stackalloc byte[] { 1, 2, 3, 4 };
        processData(in bytes);

        // scoped requiere tipo explícito
        ProcessScoped processText = (scoped ReadOnlySpan<char> text) =>
        {
            Console.WriteLine($"Processing scoped text: {text.Length} chars");
        };
        ReadOnlySpan<char> chars = "Hello".AsSpan();
        processText(chars);

        // Delegate personalizado para Span (Func no soporta ref structs)
        SumSpanDelegate sumSpan = (ReadOnlySpan<int> span) =>
        {
            int sum = 0;
            foreach (var item in span)
                sum += item;
            return sum;
        };
        ReadOnlySpan<int> numbers = stackalloc int[] { 1, 2, 3, 4, 5 };
        Console.WriteLine($"Sum: {sumSpan(numbers)}");
    }
}
