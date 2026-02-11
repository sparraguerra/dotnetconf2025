using Microsoft.EntityFrameworkCore;
using EFCore10.Models;

namespace EFCore10.Data;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Para la demo, usamos InMemory database
        // Para SQL Server con vector support: optionsBuilder.UseSqlServer("connection_string")
        optionsBuilder.UseInMemoryDatabase("BloggingDb");
        
        // EF Core 10: Logging con sensitive data (para demo)
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // EF Core 10: Complex types - Table splitting
        modelBuilder.Entity<Blog>(b =>
        {
            // Complex type mapeado a columnas en la misma tabla
            b.ComplexProperty(c => c.BillingAddress);
            
            // EF Core 10: Complex type - Con InMemory, no usamos ToJson() 
            // En SQL Server: bd => bd.ToJson()
            b.ComplexProperty(c => c.Details);
            
            // EF Core 10: Custom default constraint names (solo SQL Server)
            // b.Property(blog => blog.CreatedDate)
            //     .HasDefaultValueSql("GETDATE()", "DF_Blog_CreatedDate");
        });

        // EF Core 10: Named query filters
        modelBuilder.Entity<Post>()
            .HasQueryFilter("SoftDeletionFilter", p => !p.IsDeleted);

        // EF Core 10: Named default constraints (solo SQL Server)
        // modelBuilder.UseNamedDefaultConstraints();
    }
}
