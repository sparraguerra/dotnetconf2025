namespace CSharp14.Features;

// C# 14: Null-conditional assignment - ?. y ?[] en el lado izquierdo
internal class NullConditionalAssignment
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 14: Null-Conditional Assignment ===");

        // Ejemplo con propiedades
        Customer? customer = new() { Name = "John" };
        
        // C# 14: Assignment con ?.
        customer?.Order = GetCurrentOrder(); // Solo asigna si customer no es null
        Console.WriteLine($"Order assigned: {customer?.Order?.Id}");

        customer = null;
        customer?.Order = GetCurrentOrder(); // No hace nada, customer es null
        Console.WriteLine("Customer is null, no assignment occurred");

        // Ejemplo con indexadores
        var orders = new OrderCollection();
        orders?[0] = new Order { Id = 100 }; // Solo asigna si orders no es null
        Console.WriteLine($"Order[0] Id: {orders?[0]?.Id}");

        OrderCollection? nullOrders = null;
        nullOrders?[0] = new Order { Id = 200 }; // No hace nada
        Console.WriteLine("OrderCollection is null, no assignment occurred");

        // Compound assignment con ?.
        var account = new Account { Balance = 100 };
        account?.Balance += 50; // Solo suma si account no es null
        Console.WriteLine($"Balance after +=: {account?.Balance}");

        Account? nullAccount = null;
        nullAccount?.Balance += 50; // No hace nada
        Console.WriteLine("Account is null, no compound assignment occurred");

        // Con métodos que devuelven valores
        var inventory = new Inventory();
        inventory?.CurrentItem = GetNextItem(); // Llama GetNextItem() solo si inventory no es null
        Console.WriteLine($"Current item: {inventory?.CurrentItem}");

        // Compound con *=
        var calculator = new Calculator { Value = 5 };
        calculator?.Value *= 3;
        Console.WriteLine($"Value after *=: {calculator?.Value}");

        // Con arrays
        int[]? numbers = new int[5];
        numbers?[2] = 42; // Solo asigna si numbers no es null
        Console.WriteLine($"numbers[2]: {numbers?[2]}");

        numbers = null;
        numbers?[2] = 99; // No hace nada
        Console.WriteLine("Array is null, no assignment occurred");
    }

    static Order GetCurrentOrder()
    {
        Console.WriteLine("GetCurrentOrder() called");
        return new Order { Id = 1 };
    }

    static string GetNextItem()
    {
        Console.WriteLine("GetNextItem() called");
        return "Item-001";
    }
}

public class Customer
{
    public string Name { get; set; } = "";
    public Order? Order { get; set; }
}

public class Order
{
    public int Id { get; set; }
}

public class OrderCollection
{
    private readonly List<Order?> _orders = [null, null, null];

    public Order? this[int index]
    {
        get => _orders[index];
        set => _orders[index] = value;
    }
}

public class Account
{
    public decimal Balance { get; set; }
}

public class Inventory
{
    public string? CurrentItem { get; set; }
}

public class Calculator
{
    public int Value { get; set; }
}
