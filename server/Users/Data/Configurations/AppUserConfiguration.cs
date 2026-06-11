using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Data.Domain;

namespace Users.Data.Configurations;

/// <summary>
/// Configura el mapeo de Entity Framework Core para la entidad <see cref="AppUser"/>
/// </summary>
public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    /// <summary>
    /// Define la configuración de navegación y relación entre <see cref="AppUser"/> y su carrito
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Navigation(user => user.CartMovies)
            .HasField("_cartMovies")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(user => user.CartMovies)
            .WithOne(cartMovie => cartMovie.User)
            .HasForeignKey(ci => ci.UserId)
            .IsRequired();
    }
}
