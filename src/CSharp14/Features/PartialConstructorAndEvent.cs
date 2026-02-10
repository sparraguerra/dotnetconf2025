using System.Xml.Linq;

namespace CSharp14.Features;

// C# 14: Partial constructors y partial events
// NOTA: Esta característica requiere el compilador de C# 14 que aún no está disponible.
// Este es un ejemplo conceptual de cómo funcionará.
internal class PartialConstructorAndEvent
{
    public static void Demo()
    {
        Console.WriteLine("""
            === C# 14: Partial Constructors and Events (Conceptual) ===
            Esta característica requiere el compilador de C# 14 real.
            
            CONCEPTO:
            - Los constructores pueden ser parciales con declaración e implementación separadas
            - Los eventos pueden ser parciales con add/remove en la implementación
            - Solo la implementación puede incluir constructor initializers (this/base)
            """);

        Console.WriteLine();

        // Usando clases regulares por ahora
        var device = new Device("SmartDevice");
        device.StatusChanged += (s, e) => Console.WriteLine("Status changed!");
        device.TurnOn();
        device.TurnOff();
    }
}

 
public partial class Device
{
    // Declaración de constructor parcial
    public partial Device(string name);
    
    // Declaración de evento parcial (field-like)
    public partial event EventHandler StatusChanged;
}

public partial class Device
{
    public string Name
    {
        get;
        set;
    }
    public bool IsOn
    {
        get;
        set;
    }

    // Implementación del constructor parcial con initializer
    public partial Device(string name)
    {
        Name = name;
        IsOn = false;
    }

    // Implementación del evento parcial con add/remove
    private EventHandler? _statusChanged;    
    public partial event EventHandler StatusChanged
    {
        add
        {
            Console.WriteLine("Subscriber added");
            _statusChanged += value;
        }
        remove
        {
            Console.WriteLine("Subscriber removed");
            _statusChanged -= value;
        }
    }
    public void TurnOn()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Name} turned ON");
            _statusChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TurnOff()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Name} turned OFF");
            _statusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
} 