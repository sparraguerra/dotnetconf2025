# C# 14 vs C# 13 - Feature Comparison

Este proyecto demuestra las nuevas características de **C# 14** comparadas con **C# 13**, basado en la documentación oficial de Microsoft: [What's new in C# 14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)

## 📁 Estructura del Proyecto

```
├── CSharp13/          # Ejemplos de C# 13 (workarounds y métodos tradicionales)
│   ├── Features/      # Clases de demostración
│   └── Program.cs     # Programa principal
├── CSharp14/          # Ejemplos de C# 14 (nuevas características)
│   ├── Features/      # Clases de demostración
│   └── Program.cs     # Programa principal
└── README.md          # Este archivo
```

## ✨ Características Nuevas en C# 14

### 1. **Extension Members** 🔌
**C# 14:** Nueva sintaxis con bloques `extension` que permite:
- Extension properties (no solo métodos)
- Static extension members
- Extension operators

**C# 13:** Solo métodos de extensión tradicionales con `this`

📄 Archivos: `ExtensionBlock.cs`

### 2. **`field` Keyword** 🔑
**C# 14:** Acceso directo al backing field generado por el compilador sin declararlo explícitamente

```csharp
public string Name
{
    get;
    set => field = value ?? throw new ArgumentNullException(nameof(value));
}
```

**C# 13:** Requiere declaración explícita de backing fields

```csharp
private string _name = "";
public string Name
{
    get => _name;
    set => _name = value ?? throw new ArgumentNullException(nameof(value));
}
```

📄 Archivos: `FieldKeyword.cs`

### 3. **Implicit Span Conversions** 🔄
**C# 14:** Conversiones implícitas entre `T[]`, `Span<T>` y `ReadOnlySpan<T>`

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
ProcessSpan(numbers); // Conversión implícita
```

**C# 13:** Requiere conversiones explícitas con `.AsSpan()`

```csharp
ProcessSpan(numbers.AsSpan()); // Conversión explícita requerida
```

📄 Archivos: `ImplicitSpanConversion.cs`

### 4. **`nameof` for Unbound Generic Types** 🏷️
**C# 14:** Soporta tipos genéricos no vinculados

```csharp
Console.WriteLine(nameof(List<>));        // "List"
Console.WriteLine(nameof(Dictionary<,>)); // "Dictionary"
```

**C# 13:** Solo tipos genéricos cerrados o usar `typeof().Name`

```csharp
Console.WriteLine(nameof(List<int>));     // "List"
Console.WriteLine(typeof(List<>).Name);   // "List`1"
```

📄 Archivos: `NameofForUnboundGenerics.cs`

### 5. **Simple Lambda Parameters with Modifiers** λ
**C# 14:** Modificadores sin tipos explícitos

```csharp
TryParse<int> parse = (text, out result) => int.TryParse(text, out result);
ModifyValue doubleIt = (ref value) => value *= 2;
```

**C# 13:** Requiere tipos explícitos cuando se usan modificadores

```csharp
TryParse<int> parse = (string text, out int result) => int.TryParse(text, out result);
ModifyValue doubleIt = (ref int value) => value *= 2;
```

📄 Archivos: `SimpleLambdaParameters.cs`

### 6. **Partial Constructors and Events** 🧩
**C# 14:** Constructores y eventos pueden ser parciales

```csharp
// Declaración
public partial class Device
{
    public partial Device(string name);
    public partial event EventHandler StatusChanged;
}

// Implementación
public partial class Device
{
    public string Name { get; set; }
    public bool IsOn { get; set; }

    // Implementación del constructor parcial
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
}
```

**C# 13:** Constructores y eventos no pueden ser parciales

📄 Archivos: `PartialConstructorAndEvent.cs`

### 7. **Null-Conditional Assignment** ❓=
**C# 14:** Operadores `?.` y `?[]` en el lado izquierdo de asignaciones

```csharp
customer?.Order = GetCurrentOrder();  // Solo asigna si customer != null
account?.Balance += 50;               // Compound assignment con ?.
numbers?[2] = 42;                     // Con indexadores
```

**C# 13:** Requiere null checks explícitos

```csharp
if (customer is not null) {
    customer.Order = GetCurrentOrder();
}
if (account is not null) {
    account.Balance += 50;
}
```

📄 Archivos: `NullConditionalAssignment.cs`

### 8. **User-Defined Compound Assignment Operators** ➕=
**Nota:** Esta característica ya estaba disponible en C# 13. No hay diferencias entre las versiones.

Los operadores compuestos personalizados (`+=`, `-=`, `*=`, etc.) funcionan definiendo los operadores base (`+`, `-`, `*`).

📄 Archivos: `CompoundAssignmentOperators.cs`

## 🚀 Cómo Ejecutar

### Ejecutar proyecto C# 14:
```bash
cd CSharp14
dotnet run
```

### Ejecutar proyecto C# 13:
```bash
cd CSharp13
dotnet run
```

## 📊 Resumen de Características

| Característica | C# 13 | C# 14 |
|---------------|-------|-------|
| Extension Properties | ❌ | ✅ |
| Static Extension Members | ❌ | ✅ |
| `field` keyword | ❌ | ✅ |
| Implicit Span Conversions | ❌ | ✅ |
| `nameof` with `List<>` | ❌ | ✅ |
| Lambda modifiers sin tipos | ❌ | ✅ |
| Partial Constructors | ❌ | ✅ |
| Partial Events | ❌ | ✅ |
| Null-conditional Assignment | ❌ | ✅ |
| User-defined Compound Assignment | ✅ | ✅ |

## 🎯 Objetivos del Proyecto

- ✅ Demostrar todas las características nuevas de C# 14
- ✅ Comparar con el código equivalente en C# 13
- ✅ Proporcionar ejemplos prácticos y ejecutables
- ✅ Mostrar workarounds para características no disponibles en C# 13

## 📚 Referencias

- [What's new in C# 14 - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [C# Language Versioning](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)
- [Extension Members Specification](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions)
- [First-class Span Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-14.0/first-class-span-types)

## 💻 Requisitos

- .NET 10 SDK (para C# 14)
- .NET 8 SDK (para C# 13)
- Visual Studio 2026 o Visual Studio Code

---

**Creado para demostrar las diferencias entre C# 13 y C# 14** 🚀
