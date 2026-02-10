namespace CSharp13.Features;

// C# 13: nameof NO soporta tipos genéricos no vinculados
internal class NameOfForUnboundGenerics
{
    public static void Demo()
    {
        Console.WriteLine($"""
            === C# 13: nameof with Closed Generic Types Only ===
            
            // C# 13: NO puede usar tipos genéricos no vinculados
            // Type name: nameof(List<>)"); // ERROR en C# 13
            // Type name: nameof(Dictionary<,>)"); // ERROR en C# 13

            // C# 13: Debe usar tipos genéricos cerrados
            Type name: {nameof(List<int>)} // Output: "List"
            Type name: {nameof(Dictionary<string, int>)} // Output: "Dictionary"
            Type name: {nameof(Nullable<int>)} // Output: "Nullable"

            // Workaround: usar typeof().Name
            Using typeof: {typeof(List<>).Name} // Output: "List`1"
            Using typeof: {typeof(Dictionary<,>).Name} // Output: "Dictionary`2"

            // Para tipos personalizados, misma limitación
            Custom closed: {nameof(MyGenericClass<int>)} // Output: "MyGenericClass"
            Using typeof: {typeof(MyGenericClass<>).Name} // Output: "MyGenericClass`1"
            Using typeof: {typeof(MyGenericClass<,>).Name} // Output: "MyGenericClass`2"
            """);

        // Útil para mensajes de error y logging
        ValidateGenericType(nameof(List<int>));
        ValidateGenericType(typeof(Dictionary<,>).Name);
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
