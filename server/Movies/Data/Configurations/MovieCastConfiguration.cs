using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

/// <summary>
/// Configura el mapeo de persistencia para la entidad <see cref="MovieCast"/>
/// </summary>
internal class MovieCastConfiguration : IEntityTypeConfiguration<MovieCast>
{
    /// <summary>
    /// Define la tabla, claves, restricciones y relaciones de <see cref="MovieCast"/>
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad</param>
    public void Configure(EntityTypeBuilder<MovieCast> builder)
    {
        builder.ToTable("movies_casts", "movies");
        builder.Property(mc => mc.CharacterName)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.HasKey(mc => new { mc.MovieId, mc.PersonId });

        builder
            .HasOne(mc => mc.Movie)
            .WithMany(m => m.Casts)
            .HasForeignKey(mc => mc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(mc => mc.Person)
            .WithMany(m => m.Casts)
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
