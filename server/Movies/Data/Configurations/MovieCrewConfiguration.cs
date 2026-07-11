using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

/// <summary>
/// Configura el mapeo de persistencia para la entidad <see cref="MovieCrew"/>
/// </summary>
internal class MovieCrewConfiguration : IEntityTypeConfiguration<MovieCrew>
{
    /// <summary>
    /// Define la tabla, claves y relaciones de <see cref="MovieCrew"/>
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad</param>
    public void Configure(EntityTypeBuilder<MovieCrew> builder)
    {
        builder.ToTable("movies_crews", "movies");
        builder.HasKey(mc => new { mc.MovieId, mc.PersonId });

        builder.HasOne(mc => mc.Movie)
            .WithMany(m => m.Crews)
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mc => mc.Person)
            .WithMany(m => m.Crews)
            .HasForeignKey(m => m.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
