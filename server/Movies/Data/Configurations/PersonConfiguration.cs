using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

/// <summary>
/// Configura el mapeo de persistencia para la entidad <see cref="Person"/>
/// </summary>
internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    /// <summary>
    /// Define la tabla, claves, restricciones y relaciones de <see cref="Person"/>
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad</param>
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons", "movies");
        builder.HasKey(p => p.PersonId);
        builder.Property(p => p.PersonId).ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.Property(p => p.Bio)
                .HasMaxLength(4000);

        builder.HasMany(p => p.Casts)
            .WithOne(mc => mc.Person)
            .HasForeignKey(mc => mc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Crews)
            .WithOne(mc => mc.Person)
            .HasForeignKey(mc => mc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
