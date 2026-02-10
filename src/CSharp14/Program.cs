using CSharp14.Features;

Console.WriteLine("""
    ╔════════════════════════════════════════════════════════════╗
    ║          C# 14 - New Features Demonstration                ║
    ╚════════════════════════════════════════════════════════════╝
    """);

Console.WriteLine();

try
{
    Console.WriteLine("1. Field Keyword");
    Console.WriteLine(new string('-', 60));
    FieldKeyword.Demo();
    Console.WriteLine();

    Console.WriteLine("2. Extension Members");
    Console.WriteLine(new string('-', 60));
    ExtensionBlock.Demo();
    Console.WriteLine();

    Console.WriteLine("3. Partial Constructors and Events");
    Console.WriteLine(new string('-', 60));
    PartialConstructorAndEvent.Demo();
    Console.WriteLine();

    Console.WriteLine("4. Null-Conditional Assignment");
    Console.WriteLine(new string('-', 60));
    NullConditionalAssignment.Demo();
    Console.WriteLine();

    Console.WriteLine("5. User-Defined Compound Assignment Operators");
    Console.WriteLine(new string('-', 60));
    CompoundAssignmentOperators.Demo();
    Console.WriteLine();

    Console.WriteLine("6. Simple Lambda Parameters with Modifiers");
    Console.WriteLine(new string('-', 60));
    SimpleLambdaParameters.Demo();
    Console.WriteLine();

    Console.WriteLine("7. nameof for Unbound Generic Types");
    Console.WriteLine(new string('-', 60));
    NameOfForUnboundGenerics.Demo();
    Console.WriteLine();

    Console.WriteLine("8. Implicit Span Conversions");
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
