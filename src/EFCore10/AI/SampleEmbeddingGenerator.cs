using Microsoft.Extensions.AI;

namespace EFCore10.AI;

/// <summary>
/// Implementación de ejemplo de un generador de embeddings basado en la documentación oficial de Microsoft.
/// Genera vectores de embedding aleatorios para fines demostrativos.
/// En producción, usar OpenAI, Azure OpenAI, Ollama u otros proveedores reales.
/// </summary>
/// <remarks>
/// Fuente: https://learn.microsoft.com/es-es/dotnet/ai/iembeddinggenerator
/// </remarks>
public sealed class SampleEmbeddingGenerator(
    Uri endpoint, string modelId)
        : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly EmbeddingGeneratorMetadata _metadata =
        new("SampleEmbeddingGenerator", endpoint, modelId);

    public EmbeddingGeneratorMetadata Metadata => _metadata;

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        // Simulate some async operation.
        await Task.Delay(100, cancellationToken);

        // Create random embeddings.
        return [.. from value in values
            select new Embedding<float>(
                Enumerable.Range(0, 1536)
                .Select(_ => Random.Shared.NextSingle()).ToArray())];
    }

    public object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceKey is not null
        ? null
        : serviceType == typeof(EmbeddingGeneratorMetadata)
            ? _metadata
            : serviceType?.IsInstanceOfType(this) is true
                ? this
                : null;

    void IDisposable.Dispose() { }
}
