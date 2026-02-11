using EFCore10.Features;

// ✅ CONFIGURAR UTF-8 PARA QUE LOS EMOJIS SE MUESTREN CORRECTAMENTE
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("""
    ╔════════════════════════════════════════════════════════════=═════════════════=╗
    ║          Entity Framework Core 10 - New Features Demonstration                ║
    ╚═══════════════════════════════════════════════════════════════════════════════╝
    """);
Console.WriteLine();

try
{
    // 1. Complex Types - Table Splitting y JSON Mapping
    await ComplexTypesDemo.RunAsync();

    // 2. ExecuteUpdate con JSON Columns
    await ExecuteUpdateJsonDemo.RunAsync();

    // 3. Named Query Filters
    await NamedQueryFiltersDemo.RunAsync();

    // 4. LINQ Improvements
    await LinqImprovementsDemo.RunAsync();

    // 5. Vector Search (Conceptual)
    await VectorSearchDemo.RunAsync();

    // 6. JSON Type Support
    await JsonTypeDemo.RunAsync();    

}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

Console.WriteLine("""
    ╔════════════════════════════════════════════════════════════╗
    ║                     Demo Completed!                        ║
    ╚════════════════════════════════════════════════════════════╝
    """);