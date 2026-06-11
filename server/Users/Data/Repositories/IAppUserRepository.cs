using Users.Data.Domain;

namespace Users.Data.Repositories;

/// <summary>
/// Define las operaciones de persistencia para <see cref="AppUser"/> y su carrito
/// </summary>
public interface IAppUserRepository
{
    /// <summary>
    /// Obtiene un usuario por correo electrónico incluyendo los elementos de su carrito
    /// </summary>
    /// <param name="email">Correo electrónico del usuario a buscar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>El usuario con su carrito si existe; en caso contrario, <see langword="null"/>.</returns>
    public Task<AppUser?> GetUserWithCartByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Persiste en la base de datos los cambios pendientes en el contexto
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la finalización de la persistencia.</returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
