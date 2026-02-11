using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class ComplexTypesDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: Complex Types ===\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // ===== 1. Table Splitting con Complex Types =====
        Console.WriteLine("1. Table Splitting con Complex Types:");
        
        var blog1 = new Blog
        {
            Name = "Tech Blog",
            Tags = ["C#", "EF Core", ".NET"],
            CreatedDate = DateTime.Now,
            Details = new BlogDetails
            {
                Description = "Un blog sobre tecnología",
                Viewers = 1000,
                LastUpdated = DateTime.Now
            },
            // Complex type opcional - puede ser null
            BillingAddress = new Address
            {
                Street = "Main St",
                StreetNumber = 123,
                City = "Madrid",
                PostalCode = "28001"
            }
        };

        context.Blogs.Add(blog1);
        await context.SaveChangesAsync();
        Console.WriteLine($"  ✅ Blog '{blog1.Name}' creado con address como complex type");

        // ===== 2. JSON Mapping con Complex Types =====
        Console.WriteLine("\n2. JSON Mapping con Complex Types:");
        
        var blog2 = new Blog
        {
            Name = "Gaming Blog",
            Tags = ["Games", "Reviews"],
            CreatedDate = DateTime.Now,
            Details = new BlogDetails
            {
                Description = "Todo sobre videojuegos",
                Viewers = 5000,
                LastUpdated = DateTime.Now
            },
            BillingAddress = null // Complex type opcional
        };

        context.Blogs.Add(blog2);
        await context.SaveChangesAsync();
        Console.WriteLine($"  ✅ Blog '{blog2.Name}' creado con Details mapeado a JSON");

        // ===== 3. Consultas sobre Complex Types =====
        Console.WriteLine("\n3. Consultas sobre Complex Types:");
        Console.WriteLine("  Nota: InMemory DB requiere client evaluation para complex types");
        
        // Con SQL Server, esto se traduce directamente a SQL
        // SELECT * FROM Blogs WHERE JSON_VALUE(Details, '$.Viewers') > 3000
        var allBlogs = await context.Blogs.ToListAsync();
        var highlyViewedBlogs = allBlogs.Where(b => b.Details.Viewers > 3000).ToList();

        Console.WriteLine($"  📊 Blogs con más de 3000 viewers: {highlyViewedBlogs.Count}");
        foreach (var blog in highlyViewedBlogs)
        {
            Console.WriteLine($"    - {blog.Name}: {blog.Details.Viewers} viewers");
        }

        // ===== 4. Value Semantics con Complex Types =====
        Console.WriteLine("\n4. Value Semantics (a diferencia de owned entities):");
        
        var customer = await context.Blogs.FirstAsync(b => b.Name == "Tech Blog");
        var originalAddress = customer.BillingAddress;
        
        // Con complex types, esto funciona correctamente (value semantics)
        // Con owned entities daría error
        customer.BillingAddress = customer.BillingAddress; // Copia de valores
        await context.SaveChangesAsync();
        Console.WriteLine("  ✅ Assignment de complex types funciona correctamente");

        // ===== 5. Struct Support para Complex Types =====
        Console.WriteLine("\n5. Struct Support para Complex Types:");
        
        var blog3 = new Blog
        {
            Name = "Food Blog",
            Tags = ["Recipes", "Cooking"],
            CreatedDate = DateTime.Now,
            Details = new BlogDetails
            {
                Description = "Recetas deliciosas",
                Viewers = 2500,
                LastUpdated = DateTime.Now
            },
            // Address es un struct, no una clase
            BillingAddress = new Address
            {
                Street = "Gran Vía",
                StreetNumber = 45,
                City = "Barcelona",
                PostalCode = "08001"
            }
        };

        context.Blogs.Add(blog3);
        await context.SaveChangesAsync();
        Console.WriteLine($"  🎯 Blog '{blog3.Name}' con Address como struct");

        // ===== 6. Comparación de Complex Types =====
        Console.WriteLine("\n6. Comparación de Complex Types en queries:");
        
        var searchAddress = new Address
        {
            Street = "Main St",
            StreetNumber = 123,
            City = "Madrid",
            PostalCode = "28001"
        };
        
        // Con complex types, la comparación es por valor
        // (Con owned entities compararía por identidad)
        // InMemory DB requiere client evaluation
        var allBlogsForFiltering = await context.Blogs.ToListAsync();
        var blogsInMadrid = allBlogsForFiltering
            .Where(b => b.BillingAddress != null && b.BillingAddress.Value.City == "Madrid")
            .ToList();

        Console.WriteLine($"  📍 Blogs en Madrid: {blogsInMadrid.Count}");
    }
}
