namespace CSharp13.Features;

// C# 13: User-defined operators (mismo soporte que C# 14)
internal class CompoundAssignmentOperators
{
    public static void Demo()
    {
        Console.WriteLine("=== C# 13: User-Defined Operators (Same as C# 14) ===");

        // Los operadores compuestos personalizados ya funcionan en C# 13
        var v1 = new Vector(3, 4);
        var v2 = new Vector(1, 2);

        Console.WriteLine($"v1: {v1}");
        Console.WriteLine($"v2: {v2}");

        v1 += v2; // Ya funciona en C# 13
        Console.WriteLine($"v1 after +=: {v1}");

        v1 -= v2;
        Console.WriteLine($"v1 after -=: {v1}");

        v1 *= 3; // Escalar
        Console.WriteLine($"v1 after *= 3: {v1}");
    }
}

// Vector con operadores compuestos (ya soportado en C# 13)
public struct Vector(double x, double y)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    // Operador + que permite +=
    public static Vector operator +(Vector left, Vector right) => new(left.X + right.X, left.Y + right.Y);

    // Operador - que permite -=
    public static Vector operator -(Vector left, Vector right) => new(left.X - right.X, left.Y - right.Y);

    // Operador * que permite *= (escalar)
    public static Vector operator *(Vector vector, double scalar) => new(vector.X * scalar, vector.Y * scalar);

    public override readonly string ToString() => $"({X}, {Y})";
}
