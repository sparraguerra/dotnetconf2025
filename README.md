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


---

# 🔶 Entity Framework Core 10 - Nuevas Características 

Este proyecto de consola .NET 10 demuestra las principales características nuevas de Entity Framework Core 10, la versión LTS lanzada en noviembre de 2025.

## 🚀 Características Demostradas

### 1. **Complex Types** 📦
- **Table Splitting**: Mapear tipos complejos a columnas adicionales en la misma tabla
- **JSON Mapping**: Mapear tipos complejos a columnas JSON
- **Optional Complex Types**: Soporte para tipos complejos opcionales
- **Struct Support**: Usar structs en lugar de clases para complex types
- **Value Semantics**: Comportamiento correcto a diferencia de owned entities

### 2. **ExecuteUpdate para JSON Columns** 🔄
- Actualización eficiente de propiedades dentro de columnas JSON
- Bulk updates sin cargar datos en memoria
- Sintaxis de lambda regular (no solo expression trees)
- Soporte completo para el método `modify()` en SQL Server 2025

### 3. **Named Query Filters** 🏷️
- Múltiples filtros con nombres en una misma entidad
- Ignorar filtros específicos por nombre
- Útil para soft deletion, multi-tenancy, etc.

### 4. **Mejoras en LINQ y Traducción SQL** 🔍
- **Parameterized Collections**: Nueva estrategia con múltiples parámetros
- **Parameter Padding**: Reducción de plan cache bloat
- **LeftJoin/RightJoin**: Operadores de .NET 10 soportados
- **Split Queries**: Ordenamiento consistente mejorado
- Nuevas traducciones: DateOnly, Microsecond, Nanosecond, etc.

### 5. **Vector Search** 🤖 (SQL Server 2025/Azure SQL)
- Tipo de datos `vector(n)` para almacenar embeddings
- Función `VECTOR_DISTANCE()` para similarity search
- Ideal para RAG, semantic search y aplicaciones AI
- Métricas: cosine, euclidean, dot product

### 6. **JSON Data Type** 📄 (SQL Server 2025/Azure SQL)
- Nuevo tipo `json` nativo en lugar de `nvarchar(max)`
- Mejor rendimiento (20-30% más rápido)
- Validación automática de JSON
- Optimizaciones específicas para operaciones JSON
- Migración automática desde `nvarchar(max)`

### 7. **Otras Mejoras** ⚡
- Custom default constraint names
- Redacción de datos sensibles en logs por defecto
- Advertencias de seguridad para SQL injection
- Mejoras en la experiencia con Azure Cosmos DB

## 📋 Requisitos

- .NET 10 SDK
- Entity Framework Core 10.0.0

Para usar Vector Search y el tipo JSON nativo, necesitas:
- SQL Server 2025 o
- Azure SQL Database

## 🏃 Cómo Ejecutar

```bash
cd src/EFCore10
dotnet run
```

## 📁 Estructura del Proyecto

```
EFCore10/
├── Models/
│   ├── Blog.cs          # Entidad con complex types
│   └── Post.cs          # Entidad con soft deletion
├── Data/
│   └── BloggingContext.cs  # DbContext con configuraciones
├── Features/
│   ├── ComplexTypesDemo.cs           # Demo de complex types
│   ├── ExecuteUpdateJsonDemo.cs      # Demo de ExecuteUpdate con JSON
│   ├── NamedQueryFiltersDemo.cs      # Demo de query filters con nombre
│   ├── LinqImprovementsDemo.cs       # Demo de mejoras LINQ
│   ├── VectorSearchDemo.cs           # Demo conceptual de vector search
│   └── JsonTypeDemo.cs               # Demo del tipo JSON nativo
└── Program.cs
```

## 🔑 Conceptos Clave

### Complex Types vs Owned Entities

**Complex Types** (EF Core 10):
```csharp
modelBuilder.Entity<Blog>()
    .ComplexProperty(b => b.Details, bd => bd.ToJson());
```
- Value semantics (no identidad)
- Asignaciones funcionan correctamente
- Soportan ExecuteUpdate
- Comparaciones por valor

**Owned Entities** (versiones anteriores):
```csharp
modelBuilder.Entity<Blog>()
    .OwnsOne(b => b.Details, od => od.ToJson());
```
- Reference semantics (tienen identidad)
- Problemas con asignaciones múltiples
- No soportan ExecuteUpdate
- Comparaciones por identidad

### Vector Search

```csharp
// Definir propiedad vector
[Column(TypeName = "vector(1536)")]
public SqlVector<float> Embedding { get; set; }

// Búsqueda por similitud
var results = context.Blogs
    .OrderBy(b => EF.Functions.VectorDistance("cosine", b.Embedding, queryVector))
    .Take(5)
    .ToListAsync();
```

### Named Query Filters

```csharp
// Configuración
modelBuilder.Entity<Post>()
    .HasQueryFilter("SoftDeletionFilter", p => !p.IsDeleted)
    .HasQueryFilter("TenantFilter", p => p.TenantId == currentTenantId);

// Uso - ignorar solo soft deletion
var allPosts = context.Posts
    .IgnoreQueryFilters(["SoftDeletionFilter"])
    .ToListAsync();
```

## 📚 Recursos

- [EF Core 10 What's New](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)
- [Complex Types Documentation](https://learn.microsoft.com/en-us/ef/core/modeling/complex-types)
- [Vector Search in SQL Server](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/vector-search)
- [Query Filters](https://learn.microsoft.com/en-us/ef/core/querying/filters)

## 🎯 Casos de Uso Reales

### Complex Types
- Direcciones, información de contacto
- Configuraciones y preferencias
- Datos de auditoría
- Metadatos estructurados

### Vector Search
- Semantic search en documentos
- RAG (Retrieval-Augmented Generation)
- Sistemas de recomendación
- Búsqueda de imágenes similares

### Named Query Filters
- Soft deletion
- Multi-tenancy
- Filtros de seguridad por rol
- Filtros de regionalización

## ⚠️ Notas Importantes

1. **InMemory Database**: Esta demo usa InMemory database para simplificar. En producción:
   - Usa SQL Server 2025 o Azure SQL para vector search y JSON type
   - Configura la cadena de conexión apropiada
   - Ejecuta migraciones

2. **Migraciones**: Al actualizar de EF Core 9 a 10:
   - Las columnas `nvarchar(max)` con JSON se convertirán a tipo `json`
   - Usa `HasColumnType("nvarchar(max)")` si quieres mantener el tipo anterior

3. **Breaking Changes**: Revisa los [breaking changes](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/breaking-changes) antes de actualizar proyectos existentes

## 📊 Comparación: Complex Types vs Owned Entities

| Característica | Owned Entities | Complex Types (EF 10) |
|---------------|----------------|----------------------|
| Semantics | Reference | Value ✅ |
| Asignaciones múltiples | ❌ Error | ✅ Funciona |
| ExecuteUpdate | ❌ No soportado | ✅ Soportado |
| Comparaciones | Por identidad | Por valor ✅ |
| Struct support | ❌ | ✅ |

## 📄 Licencia

Este proyecto es de ejemplo educativo para DotNet Conf 2025.

---

**Creado para demostrar Entity Framework Core 10 - .NET 10** 🚀
