namespace CSharp13.Features;

// C# 13: Sin implicit span conversions - Conversiones explícitas requeridas
internal class ImplicitSpanConversion
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: Explicit Span Conversions ===");

        // Requiere conversión explícita a Span<T>
        int[] numbers = { 1, 2, 3, 4, 5 };
        ProcessSpan(numbers.AsSpan()); // Conversión explícita requerida

        // Requiere conversión explícita a ReadOnlySpan<T>
        ProcessReadOnlySpan(numbers.AsSpan()); // Conversión explícita

        // Span como receiver de métodos de extensión
        Span<int> span = [10, 20, 30];
        Console.WriteLine($"Span sum: {SpanExtensions.Sum(span)}"); // Llamada explícita

        // Conversión explícita de string a ReadOnlySpan<char>
        string text = "Hello";
        ProcessReadOnlySpan(text.AsSpan()); // Conversión explícita

        // Type inference requiere más ayuda
        var result = CreateSpan([1, 2, 3, 4, 5]);
        Console.WriteLine($"Created span length: {result.Length}");
    }

    // Acepta Span<T>
    static void ProcessSpan(Span<int> span)
    {
        Console.WriteLine($"Processing Span with {span.Length} elements");
        for (int i = 0; i < span.Length; i++)
        {
            span[i] *= 2;
        }
    }

    // Acepta ReadOnlySpan<T>
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

    // Type inference con arrays
    static Span<T> CreateSpan<T>(T[] items)
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
