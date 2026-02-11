using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class NamedQueryFiltersDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: Named Query Filters ===\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Crear blogs y posts de prueba
        var blog = new Blog
        {
            Name = "My Blog",
            Tags = ["General"],
            CreatedDate = DateTime.Now,
            Details = new BlogDetails
            {
                Description = "A general blog",
                Viewers = 100,
                LastUpdated = DateTime.Now
            }
        };

        var posts = new[]
        {
            new Post
            {
                Title = "Active Post 1",
                Content = "This is an active post",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Blog = blog
            },
            new Post
            {
                Title = "Active Post 2",
                Content = "Another active post",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Blog = blog
            },
            new Post
            {
                Title = "Deleted Post",
                Content = "This post was deleted",
                CreatedDate = DateTime.Now,
                IsDeleted = true,
                Blog = blog
            }
        };

        context.Blogs.Add(blog);
        context.Posts.AddRange(posts);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ 1 blog con 3 posts creados (2 activos, 1 eliminado)\n");

        // ===== 1. Query Filter aplicado por defecto =====
        Console.WriteLine("1. Query con filtro 'SoftDeletionFilter' aplicado por defecto:");
        
        var activePosts = await context.Posts.ToListAsync();
        Console.WriteLine($"  👁️ Posts visibles: {activePosts.Count}");
        foreach (var post in activePosts)
        {
            Console.WriteLine($"    - {post.Title} (IsDeleted: {post.IsDeleted})");
        }

        // ===== 2. Ignorar Named Query Filter específico =====
        Console.WriteLine("\n2. Ignorar el filtro 'SoftDeletionFilter':");
        
        var allPosts = await context.Posts
            .IgnoreQueryFilters(["SoftDeletionFilter"])
            .ToListAsync();

        Console.WriteLine($"  📊 Posts totales (incluyendo eliminados): {allPosts.Count}");
        foreach (var post in allPosts)
        {
            Console.WriteLine($"    - {post.Title} (IsDeleted: {post.IsDeleted})");
        }

        // ===== 3. Usar IgnoreQueryFilters() para todos los filtros =====
        Console.WriteLine("\n3. Ignorar TODOS los filtros:");
        
        var allPostsNoFilters = await context.Posts
            .IgnoreQueryFilters()
            .ToListAsync();

        Console.WriteLine($"  🔓 Posts sin ningún filtro: {allPostsNoFilters.Count}");

        // ===== 4. Demostrar múltiples named filters (conceptual) =====
        Console.WriteLine("\n4. Conceptualmente, con múltiples named filters:");
        Console.WriteLine("   Si tuviéramos:");
        Console.WriteLine("   - 'SoftDeletionFilter': p => !p.IsDeleted");
        Console.WriteLine("   - 'TenantFilter': p => p.TenantId == currentTenantId");
        Console.WriteLine("   ");
        Console.WriteLine("   Podríamos hacer:");
        Console.WriteLine("   context.Posts.IgnoreQueryFilters([\"TenantFilter\"])");
        Console.WriteLine("   Para ver posts de todos los tenants pero solo los no eliminados");

        // ===== 5. Query Filter en operaciones LINQ complejas =====
        Console.WriteLine("\n5. Query Filter en operaciones complejas:");
        
        var blogWithActivePosts = await context.Blogs
            .Include(b => b.Posts) // Solo incluye posts no eliminados
            .FirstAsync();

        Console.WriteLine($"  📝 Blog '{blogWithActivePosts.Name}' con {blogWithActivePosts.Posts.Count} posts activos");

        // ===== 6. Comparación con/sin filtros =====
        Console.WriteLine("\n6. Comparación de resultados:");
        
        var countWithFilter = await context.Posts.CountAsync();
        var countWithoutFilter = await context.Posts
            .IgnoreQueryFilters(["SoftDeletionFilter"])
            .CountAsync();

        Console.WriteLine($"  🔍 Posts con filtro: {countWithFilter}");
        Console.WriteLine($"  🔓 Posts sin filtro: {countWithoutFilter}");
        Console.WriteLine($"  🚫 Posts filtrados: {countWithoutFilter - countWithFilter}");
    }
}
