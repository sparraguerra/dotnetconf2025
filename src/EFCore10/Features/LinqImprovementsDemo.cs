using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class LinqImprovementsDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: LINQ Improvements ===\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Crear datos de prueba
        var blogs = new[]
        {
            new Blog
            {
                Name = "Tech Blog",
                Tags = ["C#", "Technology"],
                CreatedDate = DateTime.Now.AddDays(-10),
                Details = new BlogDetails { Description = "Tech content", Viewers = 1000, LastUpdated = DateTime.Now }
            },
            new Blog
            {
                Name = "Gaming Blog",
                Tags = ["Games", "Reviews"],
                CreatedDate = DateTime.Now.AddDays(-5),
                Details = new BlogDetails { Description = "Gaming content", Viewers = 5000, LastUpdated = DateTime.Now }
            },
            new Blog
            {
                Name = "Food Blog",
                Tags = ["Recipes", "Cooking"],
                CreatedDate = DateTime.Now.AddDays(-3),
                Details = new BlogDetails { Description = "Food content", Viewers = 3000, LastUpdated = DateTime.Now }
            }
        };

        context.Blogs.AddRange(blogs);
        
        var posts = new[]
        {
            new Post { Title = "C# 14 Features", Content = "About C# 14", CreatedDate = DateTime.Now, Blog = blogs[0] },
            new Post { Title = "EF Core 10", Content = "About EF Core", CreatedDate = DateTime.Now, Blog = blogs[0] },
            new Post { Title = "Best Games 2025", Content = "Game reviews", CreatedDate = DateTime.Now, Blog = blogs[1] },
            new Post { Title = "Pasta Recipe", Content = "How to make pasta", CreatedDate = DateTime.Now, Blog = blogs[2] }
        };

        context.Posts.AddRange(posts);
        await context.SaveChangesAsync();
Console.WriteLine("✅ Datos de prueba creados\n");

        // ===== 1. Parameterized Collections - M�ltiples par�metros (EF 10 default) =====
        Console.WriteLine("1. Parameterized Collections con m�ltiples par�metros:");
        
        int[] blogIds = [1, 2, 3];
        var selectedBlogs = await context.Blogs
            .Where(b => blogIds.Contains(b.Id))
            .ToListAsync();

        Console.WriteLine($"  📊 Blogs encontrados: {selectedBlogs.Count}");
        Console.WriteLine("  🔑 EF 10 genera: WHERE [b].[Id] IN (@ids1, @ids2, @ids3)");
        Console.WriteLine("  🔑 Evita plan cache bloat y proporciona cardinality info");

        // ===== 2. Parameterized Collections con diferentes tama�os =====
        Console.WriteLine("\n2. Parameter padding para optimizar plan cache:");
        
        int[] smallList = [1, 2];
        int[] mediumList = [1, 2, 3, 4, 5];
        
        var result1 = await context.Blogs.Where(b => smallList.Contains(b.Id)).CountAsync();
        var result2 = await context.Blogs.Where(b => mediumList.Contains(b.Id)).CountAsync();

        Console.WriteLine($"  📊 Query con 2 elementos: {result1} resultados");
        Console.WriteLine($"  📊 Query con 5 elementos: {result2} resultados");
        Console.WriteLine("  🔑 EF 10 hace padding de par�metros para reducir SQLs distintos");

        // ===== 3. Control de traducci�n de colecciones =====
        Console.WriteLine("\n3. Control expl�cito de traducci�n con EF.Constant:");
        
        string[] tags = ["C#", "Technology"];
        
        // Forzar inlining de constantes
        var blogsWithTags = await context.Blogs
            .Where(b => b.Tags.Any(t => EF.Constant(tags).Contains(t)))
            .ToListAsync();

        Console.WriteLine($"  📊 Blogs con tags espec�ficos: {blogsWithTags.Count}");
        Console.WriteLine("  🔑 EF.Constant() permite controlar el modo de traducci�n");

        // ===== 4. LeftJoin operator (conceptual con InMemory) =====
        Console.WriteLine("\n4. LEFT JOIN operator de .NET 10:");
        Console.WriteLine("  🔑 Sintaxis simplificada para LEFT JOIN");
        Console.WriteLine("  🔑 Antes: GroupJoin + SelectMany + DefaultIfEmpty");
        Console.WriteLine("  🔑 Ahora: LeftJoin m�todo directo");
        Console.WriteLine("  📝 Ejemplo conceptual:");
        Console.WriteLine("      var query = context.Blogs");
        Console.WriteLine("          .LeftJoin(context.Posts,");
        Console.WriteLine("              blog => blog.Id,");
        Console.WriteLine("              post => post.BlogId,");
        Console.WriteLine("              (blog, post) => new { blog, post });");

        // ===== 5. Split Queries con ordenamiento consistente =====
        Console.WriteLine("\n5. Split Queries con ordenamiento consistente:");
        
        var blogsWithPosts = await context.Blogs
            .AsSplitQuery()
            .Include(b => b.Posts)
            .OrderBy(b => b.Name)
            .Take(2)
            .ToListAsync();

        Console.WriteLine($"  📊 Blogs cargados con split query: {blogsWithPosts.Count}");
        Console.WriteLine("  🔑 EF 10 asegura ordenamiento consistente en todas las queries");
        foreach (var blog in blogsWithPosts)
        {
            Console.WriteLine($"    - {blog.Name} con {blog.Posts.Count} posts");
        }

        // ===== 6. Traducciones mejoradas de DateOnly =====
        Console.WriteLine("\n6. Nuevas traducciones de fecha/hora:");
        Console.WriteLine("  📅 DateOnly.ToDateTime() - Convertir DateOnly a DateTime");
        Console.WriteLine("  📅 DateOnly.DayNumber - Obtener n�mero de d�a");
        Console.WriteLine("  📅 DatePart.Microsecond y Nanosecond - Precisi�n mejorada");
        Console.WriteLine("  🔑 COALESCE optimizado como ISNULL en SQL Server");

        // ===== 7. Optimizaciones de LINQ =====
        Console.WriteLine("\n7. Optimizaciones de consultas:");
        
        var recentBlogs = await context.Blogs
            .Where(b => b.CreatedDate > DateTime.Now.AddDays(-7))
            .OrderByDescending(b => b.Details.Viewers)
            .Take(5)
            .ToListAsync();

        Console.WriteLine($"  📊 Blogs recientes: {recentBlogs.Count}");
        Console.WriteLine("  🔑 Optimización de múltiples LIMITs consecutivos");
        Console.WriteLine("  🔑 Optimización de MIN/MAX sobre DISTINCT");
        Console.WriteLine("  🔑 Nombres de parámetros simplificados (@city vs @__city_0)");

        // ===== 8. Proyecciones mejoradas =====
        Console.WriteLine("\n8. Proyecciones y operador condicional:");
        
        var blogSummaries = await context.Blogs
            .Select(b => new
            {
                b.Name,
                PostCount = b.Posts.Count, // Count optimizado
                Status = b.Details.Viewers > 2000 ? "Popular" : "Regular"
            })
            .ToListAsync();

        Console.WriteLine($"  📊 Resúmenes generados: {blogSummaries.Count}");
        foreach (var summary in blogSummaries)
        {
            Console.WriteLine($"    - {summary.Name}: {summary.PostCount} posts ({summary.Status})");
        }
        Console.WriteLine("  🔑 Optimización de Count sobre ICollection<T>");
        Console.WriteLine("  🔑 Soporte para proyectar diferentes navigations con operador condicional");
    }
}
