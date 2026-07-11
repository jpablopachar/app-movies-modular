using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

/// <summary>
/// Configura el mapeo de persistencia para la entidad <see cref="Genre"/>
/// </summary>
internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    /// <summary>
    /// Define la tabla, claves, restricciones y relaciones de <see cref="Genre"/>
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad</param>
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("genres", "movies");
        builder.HasKey(g => g.GenreId);
        builder.Property(g => g.GenreId).ValueGeneratedNever();

        builder
            .Property(g => g.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder
            .HasMany(g => g.MovieGenres)
            .WithOne(mg => mg.Genre)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
