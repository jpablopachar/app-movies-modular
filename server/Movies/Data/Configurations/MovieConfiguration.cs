using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

/// <summary>
/// Configura el mapeo de persistencia para la entidad <see cref="Movie"/>
/// </summary>
internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    /// <summary>
    /// Define la tabla, claves, restricciones y relaciones de <see cref="Movie"/>
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad</param>
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("movies", "movies");
        builder.HasKey(m => m.MovieId);
        builder.Property(m => m.MovieId).ValueGeneratedNever();

        builder
            .Property(m => m.Title)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        builder
            .Property(m => m.OriginalTitle)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        builder
            .Property(m => m.Synopsis)
            .HasMaxLength(400);

        builder.Property(m => m.Language).IsRequired();

        builder.HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Casts)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mc => mc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Crews)
            .WithOne(mc => mc.Movie)
            .HasForeignKey(mc => mc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
