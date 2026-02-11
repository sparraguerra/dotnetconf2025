using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class ExecuteUpdateJsonDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: ExecuteUpdate para JSON Columns ===\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Crear datos de prueba
        var blogs = new[]
        {
            new Blog
            {
                Name = "Tech News",
                Tags = ["Technology", "News"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "Latest tech news",
                    Viewers = 1000,
                    LastUpdated = DateTime.Now
                }
            },
            new Blog
            {
                Name = "Programming Blog",
                Tags = ["C#", "Programming"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "Programming tutorials",
                    Viewers = 5000,
                    LastUpdated = DateTime.Now
                }
            },
            new Blog
            {
                Name = "DevOps Daily",
                Tags = ["DevOps", "Cloud"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "DevOps practices",
                    Viewers = 3000,
                    LastUpdated = DateTime.Now
                }
            }
        };

        context.Blogs.AddRange(blogs);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ 3 blogs creados\n");

        // ===== 1. Actualizar una propiedad JSON =====
        Console.WriteLine("1. Actualizar viewers en JSON column:");
        
        await context.Blogs
            .Where(b => b.Name == "Tech News")
            .ExecuteUpdateAsync(s => s.SetProperty(
                b => b.Details.Viewers, 
                b => b.Details.Viewers + 500));
        
        var updatedBlog = await context.Blogs.FirstAsync(b => b.Name == "Tech News");
        Console.WriteLine($"  ✅ {updatedBlog.Name} - Viewers: {updatedBlog.Details.Viewers}");

        // ===== 2. Actualizar múltiples propiedades JSON =====
        Console.WriteLine("\n2. Actualizar múltiples propiedades en JSON:");
        
        await context.Blogs
            .Where(b => b.Details.Viewers > 2000)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Details.Viewers, b => b.Details.Viewers + 1)
                .SetProperty(b => b.Details.LastUpdated, DateTime.Now));
        
        var popularBlogs = await context.Blogs
            .Where(b => b.Details.Viewers > 2000)
            .ToListAsync();

        Console.WriteLine($"  ✅ {popularBlogs.Count} blogs populares actualizados");
        foreach (var blog in popularBlogs)
        {
            Console.WriteLine($"    - {blog.Name}: {blog.Details.Viewers} viewers");
        }

        // ===== 3. Actualización condicional en JSON =====
        Console.WriteLine("\n3. Actualización condicional basada en propiedades JSON:");
        
        await context.Blogs
            .Where(b => b.Details.Viewers < 2000)
            .ExecuteUpdateAsync(s => s.SetProperty(
                b => b.Details.Description,
                b => "Updated: " + b.Details.Description));
        
        var lowViewerBlogs = await context.Blogs
            .Where(b => b.Details.Viewers < 2000)
            .ToListAsync();

        Console.WriteLine($"  ✅ Blogs con pocos viewers actualizados:");
        foreach (var blog in lowViewerBlogs)
        {
            Console.WriteLine($"    - {blog.Name}: {blog.Details.Description}");
        }

        // ===== 4. ExecuteUpdate con lambda regular (EF Core 10 feature) =====
        Console.WriteLine("\n4. ExecuteUpdate con lambda regular (nueva sintaxis):");
        
        var shouldUpdateDescription = true;
        var shouldIncrementViewers = true;
        
        await context.Blogs
            .Where(b => b.Name.Contains("Blog"))
            .ExecuteUpdateAsync(s =>
            {
                // EF Core 10: Ahora se puede usar código imperativo
                if (shouldIncrementViewers)
                {
                    s.SetProperty(b => b.Details.Viewers, b => b.Details.Viewers + 100);
                }
                
                if (shouldUpdateDescription)
                {
                    s.SetProperty(b => b.Details.Description, "Popular blog");
                }
            });

        Console.WriteLine("  ✅ Actualización condicional completada");

        // ===== 5. Bulk update con JSON =====
        Console.WriteLine("\n5. Bulk update eficiente en JSON:");
        
        var affectedRows = await context.Blogs
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Details.LastUpdated, DateTime.Now)
                .SetProperty(b => b.Details.Viewers, b => b.Details.Viewers + 10));

        Console.WriteLine($"  ✅ {affectedRows} filas actualizadas en una sola operación");

        // Mostrar estado final
        Console.WriteLine("\n6. Estado final de todos los blogs:");
        var allBlogs = await context.Blogs.ToListAsync();
        foreach (var blog in allBlogs)
        {
            Console.WriteLine($"  - {blog.Name}:");
            Console.WriteLine($"    Description: {blog.Details.Description}");
            Console.WriteLine($"    Viewers: {blog.Details.Viewers}");
        }
    }
}
