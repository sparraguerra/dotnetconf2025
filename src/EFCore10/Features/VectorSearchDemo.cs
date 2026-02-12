using EFCore10.Data;
using EFCore10.Models;
using EFCore10.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Data.SqlTypes;
using System.Linq;

namespace EFCore10.Features;

public static class VectorSearchDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== EF Core 10: Vector Search ===\n");
        Console.WriteLine("⚠️  NOTA: Vector search requiere SQL Server 2025 o Azure SQL Database");
        Console.WriteLine("Esta demo usa embeddings simulados (en producción usar OpenAI/Azure AI)\n");

        using var context = new BloggingContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // ===== 1. Modelo con tipo Vector =====
        Console.WriteLine("1. Configuración del modelo:");
        Console.WriteLine("   public class Blog {");
        Console.WriteLine("       // Para SQL Server 2025 con tipo vector nativo:");
        Console.WriteLine("       [Column(TypeName = \"vector(1536)\")]");
        Console.WriteLine("       public float[]? Embedding { get; set; }");
        Console.WriteLine("   }");
        Console.WriteLine("   📊 1536 es el tamaño típico para embeddings de OpenAI");
        Console.WriteLine("   📝 Nota: Cuando SqlVector<T> esté disponible, usar SqlVector<float> en lugar de float[]");

        // ===== 2. Insertar embeddings con SampleEmbeddingGenerator =====
        Console.WriteLine("\n2. Insertar blogs con embeddings usando SampleEmbeddingGenerator:");
        Console.WriteLine("   ✨ Usando Microsoft.Extensions.AI con un generador de ejemplo\n");

        // Crear el generador de embeddings
        string modelId = "sample-model-v1";
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = 
            new SampleEmbeddingGenerator(
                new Uri("https://api.example.com"), 
                modelId);

        Console.WriteLine("   Generador configurado:");
        var metadata = ((SampleEmbeddingGenerator)embeddingGenerator).Metadata;
        Console.WriteLine($"   - Proveedor: {metadata.ProviderName}");
        Console.WriteLine($"   - Modelo ID: {modelId}");
        Console.WriteLine($"   - Endpoint: {metadata.ProviderUri}\n");

        // Crear blogs con embeddings generados
        var blogDescriptions = new[]
        {
            ("AI and Machine Learning", "Deep learning and neural networks", new[] { "AI", "ML" }),
            ("Web Development", "HTML, CSS, JavaScript frameworks", new[] { "Web", "Frontend" }),
            ("Data Science", "Statistical analysis and data visualization", new[] { "Data", "Analytics" }),
            ("Mobile Development", "Native and cross-platform mobile apps", new[] { "Mobile", "iOS", "Android" }),
            ("Cloud Computing", "Cloud architecture and services", new[] { "Cloud", "Azure", "AWS" })
        };

        // Generar embeddings para todos los textos
        var textsToEmbed = blogDescriptions.Select(b => b.Item2).ToArray();
        Console.WriteLine("   Generando embeddings...");
        var generatedEmbeddings = await embeddingGenerator.GenerateAsync(textsToEmbed);

        var blogs = blogDescriptions.Zip(generatedEmbeddings, (desc, embedding) => new Blog
        {
            Name = desc.Item1,
            Tags = desc.Item3,
            CreatedDate = DateTime.Now,
            PublishedDate = DateOnly.FromDateTime(DateTime.Now),
            Details = new BlogDetails
            {
                Description = desc.Item2,
                Viewers = Random.Shared.Next(2000, 7000),
                LastUpdated = DateTime.Now
            },
            // Convertir el embedding a SqlVector<float>
            Embedding = new SqlVector<float>(embedding.Vector.ToArray())
        }).ToArray();

        context.Blogs.AddRange(blogs);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ✅ {blogs.Length} blogs creados con embeddings generados");
        Console.WriteLine($"   📊 Dimensión del vector: {blogs[0].Embedding!.Value.Length}");
        Console.WriteLine("   💡 En producción:");
        Console.WriteLine("      - Usar OpenAI: new OpenAIEmbeddingGenerator(...)");
        Console.WriteLine("      - Usar Azure OpenAI: new AzureOpenAIEmbeddingGenerator(...)");
        Console.WriteLine("      - Usar Ollama: new OllamaApiClient(...)");
        Console.WriteLine($"\n   📝 Ejemplo de embedding generado para '{blogs[0].Name}':");
        var firstEmbedding = blogs[0].Embedding!.Value.Memory.Span;
        Console.WriteLine($"      [{string.Join(", ", firstEmbedding.Slice(0, Math.Min(10, firstEmbedding.Length)).ToArray())}...]");

        // ===== 3. Búsqueda por similitud con embedding generado =====
        Console.WriteLine("\n3. Búsqueda por similitud semántica:");

        // Generar embedding para la query del usuario
        string userQuery = "artificial intelligence and machine learning";
        Console.WriteLine($"   Query del usuario: \"{userQuery}\"");
        Console.WriteLine("   Generando embedding para la query...");

        var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(userQuery);
        float[] queryVector = queryEmbedding.ToArray();

        Console.WriteLine($"   ✅ Embedding generado (dimensión: {queryVector.Length})");
        Console.WriteLine($"   Embedding: [{string.Join(", ", queryVector.Take(10))}...]\n");

        // Calculamos similitud de coseno en cliente (en SQL Server 2025 sería en servidor)
        var blogsWithSimilarity = await context.Blogs
            .Where(b => b.Embedding!.HasValue)
            .ToListAsync();

        var rankedBlogs = blogsWithSimilarity
            .Select(b => new
            {
                b.Name,
                b.Details.Description,
                Similarity = CosineSimilarity(queryVector, b.Embedding!.Value)
            })
            .OrderByDescending(x => x.Similarity)
            .Take(3)
            .ToList();

        Console.WriteLine("   🔍 Top 3 blogs más similares a la query:");
        foreach (var blog in rankedBlogs)
        {
            Console.WriteLine($"      {blog.Name}: {blog.Similarity:F4} - {blog.Description}");
        }

        Console.WriteLine("\n   🔑 En SQL Server 2025:");
        Console.WriteLine("      var results = context.Blogs");
        Console.WriteLine("          .OrderBy(b => EF.Functions.VectorDistance(\"cosine\", b.Embedding, queryEmbedding))");
        Console.WriteLine("          .Take(3).ToListAsync();");

        // ===== 4. Métricas de distancia =====
        Console.WriteLine("\n4. Métricas de distancia disponibles:");
        Console.WriteLine("   - 'cosine': Similitud de coseno (más común para texto)");
        Console.WriteLine("   - 'euclidean': Distancia euclidiana");
        Console.WriteLine("   - 'dot': Producto punto\n");

        // Demostración de diferentes métricas
        var targetBlog = blogs[0]; // AI and Machine Learning
        Console.WriteLine($"   Blog de referencia: {targetBlog.Name}");
        var targetSpan = targetBlog.Embedding!.Value.Memory.Span;
        Console.WriteLine($"   Embedding: [{string.Join(", ", targetSpan.Slice(0, Math.Min(10, targetSpan.Length)).ToArray())}...]\n");

        var allBlogs = await context.Blogs.Where(b => b.Embedding!.HasValue && b.Id != targetBlog.Id).ToListAsync();

        // Cosine similarity
        Console.WriteLine("   📐 Similitud de Coseno (1 = idéntico, 0 = diferente):");
        var cosinResults = allBlogs
            .Select(b => new { b.Name, Distance = CosineSimilarity(targetBlog.Embedding!.Value, b.Embedding!.Value) })
            .OrderByDescending(x => x.Distance)
            .Take(3);

        foreach (var result in cosinResults)
        {
            Console.WriteLine($"      {result.Name}: {result.Distance:F4}");
        }

        // Euclidean distance
        Console.WriteLine("\n   📏 Distancia Euclidiana (menor = más similar):");
        var euclideanResults = allBlogs
            .Select(b => new { b.Name, Distance = EuclideanDistance(targetBlog.Embedding!.Value, b.Embedding!.Value) })
            .OrderBy(x => x.Distance)
            .Take(3);

        foreach (var result in euclideanResults)
        {
            Console.WriteLine($"      {result.Name}: {result.Distance:F4}");
        }

        // Dot product
        Console.WriteLine("\n   ⚫ Producto Punto (mayor = más similar):");
        var dotResults = allBlogs
            .Select(b => new { b.Name, Distance = DotProduct(targetBlog.Embedding!.Value, b.Embedding!.Value) })
            .OrderByDescending(x => x.Distance)
            .Take(3);

        foreach (var result in dotResults)
        {
            Console.WriteLine($"      {result.Name}: {result.Distance:F4}");
        }

        // ===== 5. Casos de uso =====
        Console.WriteLine("\n5. Casos de uso de Vector Search:");
        Console.WriteLine("   🎯 Semantic Search: Buscar por significado, no por keywords");
        Console.WriteLine("   🎯 RAG (Retrieval-Augmented Generation): Para LLMs");
        Console.WriteLine("   🎯 Recommendation Systems: Productos o contenido similar");
        Console.WriteLine("   🎯 Image Search: Búsqueda de imágenes similares");
        Console.WriteLine("   🎯 Anomaly Detection: Detectar patrones inusuales");

        // ===== 6. Híbrido: Vector Search + Filtros tradicionales =====
        Console.WriteLine("\n6. Búsqueda híbrida (Vector + Filtros tradicionales):");

        // Generar embedding para una query técnica
        string techQuery = "cloud computing and technology infrastructure";
        Console.WriteLine($"   Query técnica: \"{techQuery}\"");
        var techQueryEmbedding = await embeddingGenerator.GenerateVectorAsync(techQuery);
        float[] techQueryVector = techQueryEmbedding.ToArray();

        var hybridResults = await context.Blogs
            .Where(b => b.Embedding!.HasValue && 
                        b.Details.Viewers > 3000 && // Filtro tradicional
                        b.Tags.Any(t => t == "AI" || t == "Data" || t == "Cloud")) // Filtro por tags
            .ToListAsync();

        var rankedHybrid = hybridResults
            .Select(b => new
            {
                b.Name,
                b.Details.Viewers,
                Tags = string.Join(", ", b.Tags),
                Similarity = CosineSimilarity(techQueryVector, b.Embedding!.Value)
            })
            .OrderByDescending(x => x.Similarity)
            .ToList();

        Console.WriteLine("   🎯 Blogs técnicos con >3000 viewers ordenados por similitud:");
        foreach (var result in rankedHybrid)
        {
            Console.WriteLine($"      {result.Name} ({result.Viewers} viewers) - Similitud: {result.Similarity:F4}");
            Console.WriteLine($"         Tags: {result.Tags}");
        }

        Console.WriteLine("\n   🔑 Ventajas de búsqueda híbrida:");
        Console.WriteLine("      ✓ Combina precisión semántica con filtros estructurados");
        Console.WriteLine("      ✓ Mejora relevancia usando múltiples señales");
        Console.WriteLine("      ✓ Filtra primero, luego rankea por similitud (más eficiente)");

        // ===== 7. Implementación real con SampleEmbeddingGenerator =====
        Console.WriteLine("\n7. Implementación del SampleEmbeddingGenerator:");
        Console.WriteLine("   ");
        Console.WriteLine("   📝 Código usado en esta demo:");
        Console.WriteLine("   ");
        Console.WriteLine("   // 1. Crear el generador");
        Console.WriteLine("   IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = ");
        Console.WriteLine("       new SampleEmbeddingGenerator(");
        Console.WriteLine("           new Uri(\"https://api.example.com\"), ");
        Console.WriteLine("           \"sample-model-v1\");");
        Console.WriteLine("   ");
        Console.WriteLine("   // 2. Generar embedding de un texto");
        Console.WriteLine("   var embedding = await embeddingGenerator.GenerateVectorAsync(\"Some text\");");
        Console.WriteLine("   ");
        Console.WriteLine("   // 3. Usar el embedding en EF Core");
        Console.WriteLine("   blog.Embedding = embedding.ToArray();");
        Console.WriteLine("   await context.SaveChangesAsync();");
        Console.WriteLine("   ");
        Console.WriteLine("   // 4. Búsqueda por similitud (cliente)");
        Console.WriteLine("   var similarBlogs = blogs");
        Console.WriteLine("       .Select(b => new { b, Similarity = CosineSimilarity(queryVector, b.Embedding!) })");
        Console.WriteLine("       .OrderByDescending(x => x.Similarity)");
        Console.WriteLine("       .Take(3);");
        Console.WriteLine("\n   🔗 En SQL Server 2025 con soporte nativo:");
        Console.WriteLine("   var topSimilarBlogs = context.Blogs");
        Console.WriteLine("       .OrderBy(b => EF.Functions.VectorDistance(\"cosine\", b.Embedding, queryVector))");
        Console.WriteLine("       .Take(3).ToListAsync();");
        Console.WriteLine("\n   🔗 Proveedores de producción:");
        Console.WriteLine("      • OpenAI: Microsoft.Extensions.AI.OpenAI");
        Console.WriteLine("      • Azure OpenAI: Microsoft.Extensions.AI.AzureOpenAI");
        Console.WriteLine("      • Ollama: OllamaSharp");
        Console.WriteLine("      • Semantic Kernel: Microsoft.SemanticKernel");
        Console.WriteLine("\n   💡 SampleEmbeddingGenerator:");
        Console.WriteLine($"      - Proveedor: {metadata.ProviderName}");
        Console.WriteLine($"      - Modelo ID: {modelId}");
        Console.WriteLine($"      - Endpoint: {metadata.ProviderUri}");
        Console.WriteLine("      - Genera vectores aleatorios de 384 dimensiones");
        Console.WriteLine("      - Basado en la documentación oficial de Microsoft");

        // ===== 8. Recommendation System ejemplo =====
        Console.WriteLine("\n8. Sistema de Recomendaciones basado en vectores:");

        // Simular que un usuario está leyendo el blog "Cloud Computing"
        var userReadingBlog = await context.Blogs
            .FirstAsync(b => b.Name == "Cloud Computing");

        Console.WriteLine($"   Usuario leyendo: '{userReadingBlog.Name}'");
        Console.WriteLine("   Recomendaciones (blogs similares):\n");

        var recommendations = await context.Blogs
            .Where(b => b.Embedding!.HasValue && b.Id != userReadingBlog.Id)
            .ToListAsync();

        var topRecommendations = recommendations
            .Select(b => new
            {
                b.Name,
                b.Details.Description,
                b.Details.Viewers,
                Tags = string.Join(", ", b.Tags),
                Similarity = CosineSimilarity(userReadingBlog.Embedding!.Value, b.Embedding!.Value)
            })
            .OrderByDescending(x => x.Similarity)
            .Take(3)
            .ToList();

        foreach (var rec in topRecommendations)
        {
            Console.WriteLine($"   ⭐ {rec.Name} (Similitud: {rec.Similarity:F4})");
            Console.WriteLine($"      {rec.Description}");
            Console.WriteLine($"      {rec.Viewers} viewers | Tags: {rec.Tags}\n");
        }

        Console.WriteLine("   💡 Casos de uso:");
        Console.WriteLine("      • E-commerce: 'Productos similares a este'");
        Console.WriteLine("      • Contenido: 'Artículos relacionados'");
        Console.WriteLine("      • Streaming: 'Porque viste X, te recomendamos Y'");

        Console.WriteLine("\n🎯 Vector Search permite búsquedas semánticas potentes");
        Console.WriteLine("🎯 Esencial para aplicaciones AI modernas");
        Console.WriteLine("🎯 Disponible en SQL Server 2025 y Azure SQL Database");
    }

    // ===== Funciones helper para cálculo de similitud =====
    // En producción, estas operaciones se ejecutan en SQL Server 2025

    /// <summary>
    /// Calcula la similitud de coseno entre dos vectores
    /// Rango: 1 (idénticos) a 0 (completamente diferentes)
    /// </summary>
    private static float CosineSimilarity(float[] queryVector, SqlVector<float> blogVector)
    {
        var vector2 = blogVector.Memory.Span;

        if (queryVector.Length != vector2.Length)
            throw new ArgumentException("Los vectores deben tener la misma dimensión");

        float dotProduct = 0;
        float magnitude1 = 0;
        float magnitude2 = 0;

        for (int i = 0; i < queryVector.Length; i++)
        {
            dotProduct += queryVector[i] * vector2[i];
            magnitude1 += queryVector[i] * queryVector[i];
            magnitude2 += vector2[i] * vector2[i];
        }

        if (magnitude1 == 0 || magnitude2 == 0)
            return 0;

        return dotProduct / (MathF.Sqrt(magnitude1) * MathF.Sqrt(magnitude2));
    }

    /// <summary>
    /// Sobrecarga para dos SqlVector
    /// </summary>
    private static float CosineSimilarity(SqlVector<float> vector1, SqlVector<float> vector2)
    {
        var v1 = vector1.Memory.Span;
        var v2 = vector2.Memory.Span;

        if (v1.Length != v2.Length)
            throw new ArgumentException("Los vectores deben tener la misma dimensión");

        float dotProduct = 0;
        float magnitude1 = 0;
        float magnitude2 = 0;

        for (int i = 0; i < v1.Length; i++)
        {
            dotProduct += v1[i] * v2[i];
            magnitude1 += v1[i] * v1[i];
            magnitude2 += v2[i] * v2[i];
        }

        if (magnitude1 == 0 || magnitude2 == 0)
            return 0;

        return dotProduct / (MathF.Sqrt(magnitude1) * MathF.Sqrt(magnitude2));
    }

    /// <summary>
    /// Calcula la distancia euclidiana entre dos vectores
    /// Menor valor = más similares
    /// </summary>
    private static float EuclideanDistance(SqlVector<float> vector1, SqlVector<float> vector2)
    {
        var v1 = vector1.Memory.Span;
        var v2 = vector2.Memory.Span;

        if (v1.Length != v2.Length)
            throw new ArgumentException("Los vectores deben tener la misma dimensión");

        float sum = 0;
        for (int i = 0; i < v1.Length; i++)
        {
            float diff = v1[i] - v2[i];
            sum += diff * diff;
        }

        return MathF.Sqrt(sum);
    }

    /// <summary>
    /// Calcula el producto punto entre dos vectores
    /// Mayor valor = más similares (para vectores normalizados)
    /// </summary>
    private static float DotProduct(SqlVector<float> vector1, SqlVector<float> vector2)
    {
        var v1 = vector1.Memory.Span;
        var v2 = vector2.Memory.Span;

        if (v1.Length != v2.Length)
            throw new ArgumentException("Los vectores deben tener la misma dimensión");

        float result = 0;
        for (int i = 0; i < v1.Length; i++)
        {
            result += v1[i] * v2[i];
        }

        return result;
    }
}
