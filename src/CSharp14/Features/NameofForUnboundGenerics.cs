namespace CSharp14.Features;

// C# 14: nameof con tipos genéricos no vinculados
internal class NameOfForUnboundGenerics
{
    public static void Demo()
    {
        Console.WriteLine($"""
            === C# 14: nameof with Unbound Generic Types ===

            // C# 14: Puede usar tipos genéricos no vinculados
            Type name: {nameof(List<>)} // Output: "List"
            Type name: {nameof(Dictionary<,>)} // Output: "Dictionary"
            Type name: {nameof(Nullable<>)} // Output: "Nullable"
        
            // También funciona con tipos personalizados
            Custom type: {nameof(MyGenericClass<>)} // Output: "MyGenericClass"
            Multiple params: {nameof(MyGenericClass<,>)} // Output: "MyGenericClass"
            """);

        // Útil para mensajes de error y logging
        ValidateGenericType(nameof(List<>));
        ValidateGenericType(nameof(Dictionary<,>));

        // Comparación con tipos cerrados
        Console.WriteLine($"Closed type: {nameof(List<int>)}"); // Output: "List"
        Console.WriteLine($"Unbound type: {nameof(List<>)}"); // Output: "List"
    }

    static void ValidateGenericType(string typeName) => Console.WriteLine($"Validating generic type: {typeName}");
}

public class MyGenericClass<T>
{
    public void Method() { }
}

public class MyGenericClass<T1, T2>
{
    public void Method() { }
}
