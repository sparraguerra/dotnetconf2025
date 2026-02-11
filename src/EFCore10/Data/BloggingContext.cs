using Microsoft.EntityFrameworkCore;
using EFCore10.Models;

namespace EFCore10.Data;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // SQL Server local - Cambia la cadena de conexión según tu instalación:
        // LocalDB (Visual Studio): (localdb)\mssqllocaldb
        // SQL Express: localhost\SQLEXPRESS
        // SQL Server estándar: localhost
        optionsBuilder.UseSqlServer(
            @"Server=localhost;Database=EFCore10Demo;Trusted_Connection=True;TrustServerCertificate=True;");

        // EF Core 10: Logging con sensitive data (para demo)
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // EF Core 10: Complex types - Table splitting
        modelBuilder.Entity<Blog>(b =>
        {
            // Complex type mapeado a columnas en la misma tabla (table splitting)
            // BillingAddress se mapea como columnas individuales en la tabla Blogs
            b.ComplexProperty(e => e.BillingAddress);

            // EF Core 10: Complex type mapeado a JSON column en SQL Server
            // Details se almacena como columna JSON nativa
            b.ComplexProperty(c => c.Details, cpb =>
            {
                cpb.ToJson(); // Mapea a columna JSON en SQL Server
            });

            // EF Core 10: Custom default constraint con valor SQL
            b.Property(blog => blog.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        });

        // EF Core 10: Named query filters
        modelBuilder.Entity<Post>()
            .HasQueryFilter("SoftDeletionFilter", p => !p.IsDeleted);

        // EF Core 10: Named default constraints
        modelBuilder.UseNamedDefaultConstraints();
    }
}
