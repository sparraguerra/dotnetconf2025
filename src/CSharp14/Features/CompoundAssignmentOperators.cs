namespace CSharp14.Features;

// C# 14: User-defined compound assignment operators
internal class CompoundAssignmentOperators
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 14: User-Defined Compound Assignment ===");

        // Operadores compuestos personalizados con Vector
        var v1 = new Vector(3, 4);
        var v2 = new Vector(1, 2);

        Console.WriteLine($"v1: {v1}");
        Console.WriteLine($"v2: {v2}");

        v1 += v2; // Usa el operador += personalizado
        Console.WriteLine($"v1 after +=: {v1}");

        v1 -= v2; // Usa el operador -= personalizado
        Console.WriteLine($"v1 after -=: {v1}");

        v1 *= 3; // Usa el operador *= personalizado
        Console.WriteLine($"v1 after *= 3: {v1}"); 
    }
}

// Vector con operadores compuestos personalizados
public struct Vector(double x, double y)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    // C# 14: Operador +=    
    public void operator +=(Vector vector)
    {
        // Actualiza directamente la instancia actual. Esto es más eficiente, ya que evita asignaciones de memoria innecesarias.
        this.X += vector.X;
        this.Y += vector.Y;
    }

    // C# 14: Operador -=
    public void operator -=(Vector vector)
    {
        // Actualiza directamente la instancia actual. Esto es más eficiente, ya que evita asignaciones de memoria innecesarias.
        this.X -= vector.X;
        this.Y -= vector.Y;
    }

    // C# 14: Operador  *= (escalar)
    public void operator *=(double scalar)
    {
        // Actualiza directamente la instancia actual. Esto es más eficiente, ya que evita asignaciones de memoria innecesarias.
        this.X *= scalar;
        this.Y *= scalar;
    }

    public override readonly string ToString() => $"({X}, {Y})";
}
 