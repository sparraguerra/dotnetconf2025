using System.Linq;
namespace CSharp14.Features;

// C# 14: Extension members - Nueva sintaxis con bloques de extensión 
internal class ExtensionBlock
{
    public static void Demo()
    {
        Console.WriteLine("""
            === C# 14: Extension Members (Conceptual) ===
            Esta característica requiere el compilador de C# 14 real.

            CONCEPTO:
            - Extension properties (no solo métodos)
            - Static extension members
            - Extension operators
            """);

        Console.WriteLine();
         
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"IsEmpty: {numbers.IsEmpty}");
        Console.WriteLine($"Count: {numbers.Count}");
        Console.WriteLine($"FirstOrFallback (with fallback 0): {numbers.FirstOrFallback(0)}");

        var emptyList = new List<int>();
        Console.WriteLine($"Empty list IsEmpty: {emptyList.IsEmpty}");

        // Métodos estáticos regulares
        var list1 = new List<int> { 1, 2, 3 };
        var list2 = new List<int> { 4, 5, 6 };
        var combined = EnumerableExtensions.Combine(list1, list2);
        Console.WriteLine($"Combined: {string.Join(", ", combined)}");

        // Concat estándar en lugar de operador +
        var result = list1.Concat(list2);
        Console.WriteLine($"Using Concat: {string.Join(", ", result)}");

        // operador +
        var resultAddOperator = list1 + list2;
        Console.WriteLine($"Using Operator +: {string.Join(", ", resultAddOperator)}");
    }
}
public static class EnumerableExtensions
{
    // Extension block para instance members
    extension<TSource>(IEnumerable<TSource> source)
    {
        // Extension property (no método)
        public bool IsEmpty => !source.Any();

        // Extension method
        public TSource FirstOrFallback(TSource fallback) => source.FirstOrDefault() ?? fallback;
    }

    // Extension block para static members
    extension<TSource>(IEnumerable<TSource>)
    {
        // Static extension method
        public static IEnumerable<TSource> Combine(IEnumerable<TSource> first, IEnumerable<TSource> second) => first.Concat(second);

        // Static extension property
        public static IEnumerable<TSource> Identity => [];

        // Static extension operator
        public static IEnumerable<TSource> operator +(IEnumerable<TSource> left, IEnumerable<TSource> right) => left.Concat(right);
    }
}