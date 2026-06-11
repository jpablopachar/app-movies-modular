using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Data.Domain;

namespace Users.Data.Configurations;

/// <summary>
/// Configura el mapeo de Entity Framework Core para la entidad <see cref="CartMovie"/>
/// </summary>
public sealed class CartMovieConfiguration : IEntityTypeConfiguration<CartMovie>
{
    /// <summary>
    /// Define tabla, clave compuesta y relación con <see cref="AppUser"/> para los elementos del carrito
    /// </summary>
    /// <param name="builder">Constructor de configuración de la entidad.</param>
    public void Configure(EntityTypeBuilder<CartMovie> builder)
    {
        builder.ToTable("cart_movies", "users");
        builder.HasKey(cartMovie => new { cartMovie.UserId, cartMovie.MovieId });

        builder.HasOne(cartMovie => cartMovie.User)
            .WithMany(user => user.CartMovies)
            .HasForeignKey(cartMovie => cartMovie.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
