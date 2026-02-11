using EFCore10.Data;
using EFCore10.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore10.Features;

public static class VectorSearchDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: Vector Search (Conceptual) ===\n");
Console.WriteLine("⚠️  NOTA: Vector search requiere SQL Server 2025 o Azure SQL Database");
        Console.WriteLine("Esta demo muestra el concepto, pero usa InMemory database\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // ===== 1. Modelo con tipo Vector =====
        Console.WriteLine("1. Configuraci�n del modelo:");
        Console.WriteLine("   public class Blog {");
        Console.WriteLine("       [Column(TypeName = \"vector(1536)\")]");
        Console.WriteLine("       public SqlVector<float>? Embedding { get; set; }");
         Console.WriteLine("   }");
         Console.WriteLine("   📊 vector(1536) es el tama�o t�pico para embeddings de OpenAI");

        // ===== 2. Insertar embeddings =====
        Console.WriteLine("\n2. Insertar blogs con embeddings:");
        Console.WriteLine("   // Con un embedding generator real:");
        Console.WriteLine("   var embedding = await embeddingGenerator.GenerateVectorAsync(text);");
        Console.WriteLine("   blog.Embedding = new SqlVector<float>(embedding);");
        Console.WriteLine("   await context.SaveChangesAsync();");

        // Datos simulados para la demo
        var blogs = new[]
        {
            new Blog
            {
                Name = "AI and Machine Learning",
                Tags = ["AI", "ML"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "Deep learning and neural networks",
                    Viewers = 5000,
                    LastUpdated = DateTime.Now
                }
            },
            new Blog
            {
                Name = "Web Development",
                Tags = ["Web", "Frontend"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "HTML, CSS, JavaScript frameworks",
                    Viewers = 3000,
                    LastUpdated = DateTime.Now
                }
            },
            new Blog
            {
                Name = "Data Science",
                Tags = ["Data", "Analytics"],
                CreatedDate = DateTime.Now,
                Details = new BlogDetails
                {
                    Description = "Statistical analysis and data visualization",
                    Viewers = 4000,
                    LastUpdated = DateTime.Now
                }
            }
        };

        context.Blogs.AddRange(blogs);
        await context.SaveChangesAsync();
        Console.WriteLine("   ✅ 3 blogs creados (en producci�n con embeddings)");

        // ===== 3. Similarity Search con VectorDistance =====
        Console.WriteLine("\n3. B�squeda por similitud sem�ntica:");
        Console.WriteLine("   var queryEmbedding = /* vector de la query del usuario */;");
        Console.WriteLine("   var similarBlogs = context.Blogs");
        Console.WriteLine("       .OrderBy(b => EF.Functions.VectorDistance(");
        Console.WriteLine("           \"cosine\", b.Embedding, queryEmbedding))");
        Console.WriteLine("       .Take(3)");
        Console.WriteLine("       .ToListAsync();");
        Console.WriteLine("   ");
        Console.WriteLine("   🔍 Genera SQL: ORDER BY VECTOR_DISTANCE('cosine', ...)");

        // ===== 4. M�tricas de distancia =====
        Console.WriteLine("\n4. M�tricas de distancia disponibles:");
        Console.WriteLine("   - 'cosine': Similitud de coseno (m�s com�n para texto)");
        Console.WriteLine("   - 'euclidean': Distancia euclidiana");
        Console.WriteLine("   - 'dot': Producto punto");

        // ===== 5. Casos de uso =====
        Console.WriteLine("\n5. Casos de uso de Vector Search:");
        Console.WriteLine("   🎯 Semantic Search: Buscar por significado, no por keywords");
        Console.WriteLine("   🎯 RAG (Retrieval-Augmented Generation): Para LLMs");
        Console.WriteLine("   🎯 Recommendation Systems: Productos o contenido similar");
        Console.WriteLine("   🎯 Image Search: B�squeda de im�genes similares");
        Console.WriteLine("   🎯 Anomaly Detection: Detectar patrones inusuales");

        // ===== 6. Integraci�n con AI =====
        Console.WriteLine("\n6. Integraci�n con generadores de embeddings:");
        Console.WriteLine("   // Usando Microsoft.Extensions.AI");
        Console.WriteLine("   IEmbeddingGenerator<string, Embedding<float>> generator = ...;");
        Console.WriteLine("   ");
        Console.WriteLine("   // Generar embedding de texto de usuario");
        Console.WriteLine("   var userQuery = \"machine learning tutorials\";");
        Console.WriteLine("   var queryVector = await generator.GenerateVectorAsync(userQuery);");
        Console.WriteLine("   ");
        Console.WriteLine("   // Buscar blogs similares");
        Console.WriteLine("   var results = await context.Blogs");
        Console.WriteLine("       .OrderBy(b => EF.Functions.VectorDistance(");
        Console.WriteLine("           \"cosine\", b.Embedding, sqlVector))");
        Console.WriteLine("       .Take(5)");
        Console.WriteLine("       .ToListAsync();");

        // ===== 7. Performance =====
        Console.WriteLine("\n7. Consideraciones de rendimiento:");
        Console.WriteLine("   ? �ndices vectoriales optimizados en SQL Server 2025");
        Console.WriteLine("   ? B�squeda eficiente incluso con millones de vectores");
        Console.WriteLine("   ⚡ Combinar con filtros tradicionales para mejores resultados");
        Console.WriteLine("   ");
        Console.WriteLine("   Ejemplo con filtros:");
        Console.WriteLine("   var results = context.Blogs");
        Console.WriteLine("       .Where(b => b.Details.Viewers > 1000) // Filtro tradicional");
        Console.WriteLine("       .OrderBy(b => EF.Functions.VectorDistance(...)) // Vector search");
        Console.WriteLine("       .Take(10);");

        // ===== 8. Hybrid Search conceptual =====
        Console.WriteLine("\n8. Hybrid Search (Vector + Full-text):");
        Console.WriteLine("   En Azure Cosmos DB se puede combinar:");
        Console.WriteLine("   - Vector similarity search");
        Console.WriteLine("   - Full-text search");
        Console.WriteLine("   Usando la funci�n RRF (Reciprocal Rank Fusion)");

Console.WriteLine("\n🎯 Vector Search permite b�squedas sem�nticas potentes");
Console.WriteLine("🎯 Esencial para aplicaciones AI modernas");
Console.WriteLine("🎯 Disponible en SQL Server 2025 y Azure SQL Database");
    }
}
