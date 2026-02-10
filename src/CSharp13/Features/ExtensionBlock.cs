using System.Linq;

namespace CSharp13.Features;

// C# 13: Sin extension members - Métodos de extensión tradicionales
internal class ExtensionBlock
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: Traditional Extension Methods ===");

        // Solo métodos de extensión, no propiedades
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"IsEmpty: {numbers.IsEmpty()}"); // Método, no property
        Console.WriteLine($"Count: {numbers.Count}");
        Console.WriteLine($"FirstOrFallback (with fallback 0): {numbers.FirstOrFallback(0)}");

        var emptyList = new List<int>();
        Console.WriteLine($"Empty list IsEmpty: {emptyList.IsEmpty()}");

        // No hay static extension members - usar métodos estáticos regulares
        var list1 = new List<int> { 1, 2, 3 };
        var list2 = new List<int> { 4, 5, 6 };
        var combined = EnumerableExtensions.Combine(list1, list2); // Método estático regular
        Console.WriteLine($"Combined: {string.Join(", ", combined)}");

        // No hay operadores de extensión
        var result = list1.Concat(list2); // Usa Concat estándar
        Console.WriteLine($"Using Concat: {string.Join(", ", result)}");

        // Propiedad estática regular
        var identity = EnumerableExtensions.GetIdentity<int>();
        Console.WriteLine($"Identity IsEmpty: {identity.IsEmpty()}");
    }
}

// Métodos de extensión tradicionales de C# 13
public static class EnumerableExtensions
{
    // Extension method tradicional (no property)
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => !source.Any();

    // Extension method tradicional (no property)
    public static TSource FirstOrFallback<TSource>(this IEnumerable<TSource> source, TSource fallback) => source.FirstOrDefault() ?? fallback;

    // Método estático regular (no es extension)
    public static IEnumerable<TSource> Combine<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        foreach (var item in first)
            yield return item;
        foreach (var item in second)
            yield return item;
    }

    // Método estático regular para obtener identidad
    public static IEnumerable<TSource> GetIdentity<TSource>() => [];

    // No se pueden definir operadores como extensiones en C# 13
}

