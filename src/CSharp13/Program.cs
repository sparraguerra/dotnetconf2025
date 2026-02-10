using CSharp13.Features;

Console.WriteLine("""
    ╔════════════════════════════════════════════════════════════╗
    ║    C# 13 - Comparison with C# 14 Features (Workarounds)    ║
    ╚════════════════════════════════════════════════════════════╝
    """);

Console.WriteLine();

try
{
    Console.WriteLine("1. Explicit Backing Fields (vs Field Keyword)");
    Console.WriteLine(new string('-', 60));
    FieldKeyword.Demo();
    Console.WriteLine();

    Console.WriteLine("2. Traditional Extension Methods (vs Extension Members)");
    Console.WriteLine(new string('-', 60));
    ExtensionBlock.Demo();
    Console.WriteLine();

    Console.WriteLine("3. Regular Constructors/Events (vs Partial)");
    Console.WriteLine(new string('-', 60));
    PartialConstructorAndEvent.Demo();
    Console.WriteLine();

    Console.WriteLine("4. Explicit Null Checks (vs Null-Conditional Assignment)");
    Console.WriteLine(new string('-', 60));
    NullConditionalAssignment.Demo();
    Console.WriteLine();

    Console.WriteLine("5. User-Defined Operators (Already Supported)");
    Console.WriteLine(new string('-', 60));
    CompoundAssignmentOperators.Demo();
    Console.WriteLine();

    Console.WriteLine("6. Lambda Parameters with Explicit Types (vs Simple)");
    Console.WriteLine(new string('-', 60));
    SimpleLambdaParameters.Demo();
    Console.WriteLine();

    Console.WriteLine("7. nameof with Closed Generic Types (vs Unbound)");
    Console.WriteLine(new string('-', 60));
    NameOfForUnboundGenerics.Demo();
    Console.WriteLine();

    Console.WriteLine("8. Explicit Span Conversions (vs Implicit)");
    Console.WriteLine(new string('-', 60));
    ImplicitSpanConversion.Demo();
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}

Console.WriteLine("""
    ╔════════════════════════════════════════════════════════════╗
    ║                     Demo Completed!                        ║
    ╚════════════════════════════════════════════════════════════╝
    """);
