namespace CSharp13.Features;

// C# 13: Sin partial constructors ni partial events
internal class PartialConstructorAndEvent
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: No Partial Constructors or Events ===");

        var device = new Device("SmartDevice");
        device.TurnOn();
        device.TurnOff();
    }
}

// Clase parcial - constructor no puede ser parcial
public partial class Device
{
    private string _name;
    private bool _isOn;
    private EventHandler? _statusChanged;

    // C# 13: Constructor regular (no parcial)
    public Device(string name)
    {
        _name = name;
        _isOn = false;
        Console.WriteLine($"Device '{_name}' created");
    }

    // C# 13: Evento con backing field explícito y accessors manuales
    public event EventHandler StatusChanged
    {
        add
        {
            Console.WriteLine("StatusChanged subscriber added");
            OnStatusChangedAdded(value);
            _statusChanged += value;
        }
        remove
        {
            Console.WriteLine("StatusChanged subscriber removed");
            OnStatusChangedRemoved(value);
            _statusChanged -= value;
        }
    }

    private partial void OnStatusChangedAdded(EventHandler handler);
    private partial void OnStatusChangedRemoved(EventHandler handler);

    public void TurnOn()
    {
        if (!_isOn)
        {
            _isOn = true;
            Console.WriteLine($"{_name} turned ON");
            OnStatusChanged();
        }
    }

    public void TurnOff()
    {
        if (_isOn)
        {
            _isOn = false;
            Console.WriteLine($"{_name} turned OFF");
            OnStatusChanged();
        }
    }

   
}

// Segunda parte de la clase parcial
public partial class Device
{
    // Otros miembros pueden estar aquí, pero no partes del constructor o evento
    private void OnStatusChanged()
    {
        _statusChanged?.Invoke(this, EventArgs.Empty);
    }

    private partial void OnStatusChangedAdded(EventHandler handler)
    {
        _statusChanged?.Invoke(this, EventArgs.Empty);
    }

    private partial void OnStatusChangedRemoved(EventHandler handler)
    {
        _statusChanged?.Invoke(this, EventArgs.Empty);
    }
} 
