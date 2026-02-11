namespace CSharp14.Features;

// C# 14: Implicit span conversions - Conversiones implícitas mejoradas
internal class ImplicitSpanConversion
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 14: Implicit Span Conversions ===");

        // Conversión implícita de array a Span<T>
        int[] numbers = { 1, 2, 3, 4, 5 };
        ProcessSpan(numbers); // Conversión implícita

        // Conversión implícita de array a ReadOnlySpan<T>
        ProcessReadOnlySpan(numbers); // Conversión implícita

        // Span como receiver de métodos de extensión
        Span<int> span = [10, 20, 30];
        Console.WriteLine($"Span sum: {span.Sum()}"); // Extension method

        // Composición con otras conversiones
        string text = "Hello";
        ProcessReadOnlySpan(text); // string -> ReadOnlySpan<char>

        // Type inference mejorado
        var result = CreateSpan(1, 2, 3, 4, 5);
        Console.WriteLine($"Created span length: {result.Length}");
    }

    // Acepta Span<T> directamente
    static void ProcessSpan(Span<int> span)
    {
        Console.WriteLine($"Processing Span with {span.Length} elements");
        for (int i = 0; i < span.Length; i++)
        {
            span[i] *= 2;
        }
    }

    // Acepta ReadOnlySpan<T> directamente
    static void ProcessReadOnlySpan(ReadOnlySpan<int> span)
    {
        Console.WriteLine($"Processing ReadOnlySpan with {span.Length} elements");
        foreach (var item in span)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
    }

    // Sobrecarga para char spans
    static void ProcessReadOnlySpan(ReadOnlySpan<char> span)
    {
        Console.WriteLine($"Processing string as ReadOnlySpan: {span.Length} chars");
    }

    // Type inference mejorado con spans
    static Span<T> CreateSpan<T>(params T[] items)
    {
        return items.AsSpan();
    }
}

// Métodos de extensión para Span<T>
public static class SpanExtensions
{
    public static int Sum(this Span<int> span)
    {
        int sum = 0;
        foreach (var item in span)
        {
            sum += item;
        }
        return sum;
    }
}
