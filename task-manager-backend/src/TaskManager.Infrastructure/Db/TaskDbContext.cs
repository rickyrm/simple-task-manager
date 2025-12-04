using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Db
{
    /// <summary>
    /// Contexto de EF Core para las tareas.
    /// - Define el DbSet `Tasks` que mapea la entidad `TaskItem`.
    /// - Configuraciones (constraints, índices, defaults) se aplican en `OnModelCreating`.
    /// La cadena de conexión y el proveedor (SQLite) se configuran en `Program.cs`.
    /// </summary>
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description)
                      .HasMaxLength(1000);

                entity.Property(e => e.IsCompleted)
                      .HasDefaultValue(false);

                // `CreatedAt` por defecto con la marca de tiempo actual en SQLite.
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Índice para filtrar rápido por `IsCompleted` en queries.
                entity.HasIndex(e => e.IsCompleted);
            });
        }
    }
}
