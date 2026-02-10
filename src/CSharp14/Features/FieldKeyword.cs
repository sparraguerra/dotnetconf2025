namespace CSharp14.Features;

// C# 14: field keyword - Acceso al backing field generado por el compilador
internal class FieldKeyword
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 14: field Keyword ===");

        var person = new Person
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        Console.WriteLine($"Name: {person.Name}");
        Console.WriteLine($"Email: {person.Email}");

        try
        {
            person.Name = null!; // Lanzará excepción
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        var counter = new Counter { Value = 5 };
        Console.WriteLine($"Initial: {counter.Value}");
        counter.Value = 10;
        Console.WriteLine($"After update: {counter.Value}");
    }
}

public class Person
{
    // C# 14: Usa 'field' sin declarar backing field explícito
    public required string Name
    {
        get;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }

    // También funciona en ambos accessors
    public string Email
    {
        get => field?.ToLower() ?? "";
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class Counter
{
    // Validación y transformación con field
    public int Value { get; set; }
}
