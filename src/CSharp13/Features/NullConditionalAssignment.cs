namespace CSharp13.Features;
// C# 13: Sin null-conditional assignment - Requiere null checks explícitos

internal class NullConditionalAssignment
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: Explicit Null Checks for Assignment ===");

        // Ejemplo con propiedades
        Customer? customer = new() { Name = "John" };

        // C# 13: Requiere if explícito
        if (customer is not null)
        {
            customer.Order = GetCurrentOrder();
        }
        Console.WriteLine($"Order assigned: {customer?.Order?.Id}");

        customer = null;
        if (customer is not null)
        {
            customer.Order = GetCurrentOrder(); // No se ejecuta
        }
        Console.WriteLine("Customer is null, no assignment occurred");

        // Ejemplo con indexadores
        var orders = new OrderCollection();
        if (orders is not null)
        {
            orders[0] = new Order { Id = 100 };
        }
        Console.WriteLine($"Order[0] Id: {orders?[0]?.Id}");

        OrderCollection? nullOrders = null;
        if (nullOrders is not null)
        {
            nullOrders[0] = new Order { Id = 200 }; // No se ejecuta
        }
        Console.WriteLine("OrderCollection is null, no assignment occurred");

        // Compound assignment requiere null check
        var account = new Account { Balance = 100 };
        if (account is not null)
        {
            account.Balance += 50;
        }
        Console.WriteLine($"Balance after +=: {account?.Balance}");

        Account? nullAccount = null;
        if (nullAccount is not null)
        {
            nullAccount.Balance += 50; // No se ejecuta
        }
        Console.WriteLine("Account is null, no compound assignment occurred");

        // Con métodos que devuelven valores
        var inventory = new Inventory();
        if (inventory is not null)
        {
            inventory.CurrentItem = GetNextItem();
        }
        Console.WriteLine($"Current item: {inventory?.CurrentItem}");

        // Compound con *=
        var calculator = new Calculator { Value = 5 };
        if (calculator is not null)
        {
            calculator.Value *= 3;
        }
        Console.WriteLine($"Value after *=: {calculator?.Value}");

        // Con arrays
        int[]? numbers = new int[5];
        if (numbers is not null)
        {
            numbers[2] = 42;
        }
        Console.WriteLine($"numbers[2]: {numbers?[2]}");

        numbers = null;
        if (numbers is not null)
        {
            numbers[2] = 99; // No se ejecuta
        }
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
