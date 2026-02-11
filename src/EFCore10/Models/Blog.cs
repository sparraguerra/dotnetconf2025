using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore10.Models;

public class Blog
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTime CreatedDate { get; set; }
    
    // EF Core 10: Complex types - Optional complex type
    public Address? BillingAddress { get; set; }
    
    // EF Core 10: Complex types - Required complex type mapped to JSON
    public required BlogDetails Details { get; set; }
    
    // EF Core 10: Vector search support (SQL Server 2025)
    // Nota: SqlVector requiere Microsoft.Data.SqlClient con soporte para SQL Server 2025
    // [Column(TypeName = "vector(1536)")]
    // public SqlVector<float>? Embedding { get; set; }
    
    public ICollection<Post> Posts { get; set; } = [];
}

// EF Core 10: Complex type - puede ser struct o class
public class BlogDetails
{
    public string? Description { get; set; }
    public int Viewers { get; set; }
    public DateTime LastUpdated { get; set; }
}

// EF Core 10: Struct support para complex types
public struct Address
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public int StreetNumber { get; set; }
}
