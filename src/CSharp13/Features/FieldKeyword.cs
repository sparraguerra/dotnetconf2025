namespace CSharp13.Features;

// C# 13: Sin field keyword - Backing fields explícitos requeridos
internal class FieldKeyword
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: Explicit Backing Fields ===");

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
    // C# 13: Requiere declaración explícita de backing fields
    private string _name = "";
    public required string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    private string _email = "";
    public string Email
    {
        get => _email?.ToLower() ?? "";
        set => _email = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class Counter
{
    public int Value { get; set; }
}
