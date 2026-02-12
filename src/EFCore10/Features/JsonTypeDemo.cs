using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class JsonTypeDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: JSON Data Type Support ===\n");
        Console.WriteLine("⚠️  NOTA: El nuevo tipo 'json' requiere SQL Server 2025 o Azure SQL");
        Console.WriteLine("Esta demo usa SQL Server\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // ===== 1. Diferencia entre nvarchar(max) y json type =====
        Console.WriteLine("1. Nuevo tipo de datos 'json' (SQL Server 2025):");
        Console.WriteLine("   Antes (SQL Server 2016-2022):");
        Console.WriteLine("   CREATE TABLE Blogs ([Details] nvarchar(max))");
        Console.WriteLine("   ");
        Console.WriteLine("   Ahora (SQL Server 2025):");
        Console.WriteLine("   CREATE TABLE Blogs ([Details] json NOT NULL)");
        Console.WriteLine("   ");
        Console.WriteLine("   ⚡ Mejor rendimiento");
        Console.WriteLine("   ✅ Validación de JSON a nivel de base de datos");
        Console.WriteLine("   🎯 Optimizaciones específicas para JSON");

        // ===== 2. Configuración automática =====
        Console.WriteLine("\n2. Configuración automática en EF Core 10:");
        Console.WriteLine("   protected override void OnConfiguring(DbContextOptionsBuilder options)");
        Console.WriteLine("   {");
        Console.WriteLine("       options.UseSqlServer(connection, o => o.UseAzureSql());");
        Console.WriteLine("       // o compatibility level >= 170 (SQL Server 2025)");
        Console.WriteLine("   }");
        Console.WriteLine("   ");
        Console.WriteLine("   ✅ EF 10 usa automáticamente el tipo 'json'");
        Console.WriteLine("   🎯 No se requiere configuración adicional");

        // ===== 3. Crear datos con JSON =====
        Console.WriteLine("\n3. Trabajar con columnas JSON:");
        
        var blogs = new[]
        {
            new Blog
            {
                Name = "Tech Blog",
                Tags = ["C#", "EF Core", "SQL"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "A technical blog about .NET",
                    Viewers = 1500,
                    LastUpdated = DateTime.Now
                }
            },
            new Blog
            {
                Name = "Gaming Blog",
                Tags = ["Games", "Reviews", "News"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "Latest gaming news and reviews",
                    Viewers = 5000,
                    LastUpdated = DateTime.Now
                }
            }
        };

        context.Blogs.AddRange(blogs);
        await context.SaveChangesAsync();
        Console.WriteLine("   ✅ Blogs con datos JSON creados");

        // ===== 4. Consultas sobre JSON =====
        Console.WriteLine("\n4. Consultas LINQ sobre propiedades JSON:");
        
        var popularBlogs = await context.Blogs
            .Where(b => b.Details.Viewers > 2000)
            .ToListAsync();

        Console.WriteLine($"   📊 Blogs populares (>2000 viewers): {popularBlogs.Count}");
        foreach (var blog in popularBlogs)
        {
            Console.WriteLine($"     - {blog.Name}: {blog.Details.Viewers} viewers");
        }

        Console.WriteLine("\n   SQL generado:");
        Console.WriteLine("   SELECT [b].[Id], [b].[Name], [b].[Details]");
        Console.WriteLine("   FROM [Blogs] AS [b]");
        Console.WriteLine("   WHERE JSON_VALUE([b].[Details], '$.Viewers' RETURNING int) > 2000");
        Console.WriteLine("   ");
        Console.WriteLine("   🔑 Usa JSON_VALUE() con RETURNING clause");

        // ===== 5. Primitive Collections =====
        Console.WriteLine("\n5. Primitive Collections como JSON:");
        
        var blogsWithCSharp = await context.Blogs
            .Where(b => b.Tags.Contains("C#"))
            .ToListAsync();

        Console.WriteLine($"   📊 Blogs con tag 'C#': {blogsWithCSharp.Count}");
        Console.WriteLine("   ");
        Console.WriteLine("   CREATE TABLE Blogs ([Tags] json NOT NULL)");
        Console.WriteLine("   🎯 Arrays de strings almacenados como JSON");

        // ===== 6. Complex Types en JSON =====
        Console.WriteLine("\n6. Complex Types mapeados a JSON:");
        Console.WriteLine("   modelBuilder.Entity<Blog>()");
        Console.WriteLine("       .ComplexProperty(b => b.Details, bd => bd.ToJson());");
        Console.WriteLine("   ");
        Console.WriteLine("   ✅ Datos estructurados en una sola columna");
        Console.WriteLine("   ⚡ Consultas eficientes sobre propiedades anidadas");
        Console.WriteLine("   🔄 Actualizaciones parciales con ExecuteUpdate");

        // ===== 7. Actualizaciones =====
        Console.WriteLine("\n7. Actualizar datos JSON:");
        
        await context.Blogs
            .Where(b => b.Name == "Tech Blog")
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Details.Viewers, b => b.Details.Viewers + 100));

        var updated = await context.Blogs.FirstAsync(b => b.Name == "Tech Blog");
        Console.WriteLine($"   ✅ Viewers actualizados: {updated.Details.Viewers}");
        Console.WriteLine("   ");
        Console.WriteLine("   SQL generado (SQL Server 2025):");
        Console.WriteLine("   UPDATE [Blogs]");
        Console.WriteLine("   SET [Details].modify('$.Viewers',");
        Console.WriteLine("       JSON_VALUE([Details], '$.Viewers' RETURNING int) + 100)");
        Console.WriteLine("   WHERE [Name] = 'Tech Blog'");

        // ===== 8. Migración desde nvarchar =====
        Console.WriteLine("\n8. Migración de nvarchar(max) a json:");
        Console.WriteLine("   ⚠️  Primera migración después de actualizar a EF 10:");
        Console.WriteLine("   ");
        Console.WriteLine("   migrationBuilder.AlterColumn<string>(");
        Console.WriteLine("       name: \"Details\",");
        Console.WriteLine("       table: \"Blogs\",");
        Console.WriteLine("       type: \"json\",");
        Console.WriteLine("       nullable: false,");
        Console.WriteLine("       oldClrType: typeof(string),");
        Console.WriteLine("       oldType: \"nvarchar(max)\");");
        Console.WriteLine("   ");
        Console.WriteLine("   Para mantener nvarchar(max):");
        Console.WriteLine("   property.HasColumnType(\"nvarchar(max)\")");

        // ===== 9. Beneficios =====
        Console.WriteLine("\n9. Beneficios del tipo 'json':");
        Console.WriteLine("   ⚡ Rendimiento: 20-30% más rápido en operaciones JSON");
        Console.WriteLine("   ✅ Validación: JSON inválido rechazado a nivel de DB");
        Console.WriteLine("   📑 Índices: Mejores opciones de indexación");
        Console.WriteLine("   🔧 Compatibilidad: Funciones JSON optimizadas");
        Console.WriteLine("   💾 Tamaño: Almacenamiento más eficiente");

        // ===== 10. Consultas complejas =====
        Console.WriteLine("\n10. Consultas complejas sobre JSON:");
        
        var blogSummaries = await context.Blogs
            .OrderByDescending(b => b.Details.Viewers)
            .Select(b => new
            {
                b.Name,
                b.Details.Description,
                b.Details.Viewers,
                TagCount = b.Tags.Length,
                IsPopular = b.Details.Viewers > 3000
            })
            .ToListAsync();

        Console.WriteLine($"   📊 Resúmenes generados: {blogSummaries.Count}");
        foreach (var summary in blogSummaries)
        {
            Console.WriteLine($"     - {summary.Name}:");
            Console.WriteLine($"       Viewers: {summary.Viewers}, Tags: {summary.TagCount}");
            Console.WriteLine($"       Popular: {summary.IsPopular}");
        }

        Console.WriteLine("\n🎯 El tipo 'json' mejora significativamente el rendimiento");
        Console.WriteLine("🎯 Transición automática en EF Core 10");
        Console.WriteLine("🎯 Disponible en SQL Server 2025 y Azure SQL Database");
    }
}
