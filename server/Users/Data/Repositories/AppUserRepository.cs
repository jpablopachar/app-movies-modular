using Microsoft.EntityFrameworkCore;
using Users.Data.Domain;

namespace Users.Data.Repositories;

/// <summary>
/// Repositorio para consultar y persistir usuarios de aplicación
/// </summary>
/// <param name="context">Contexto de datos de usuarios utilizado para el acceso a persistencia.</param>
public class AppUserRepository(UserDbContext context) : IAppUserRepository
{
    private readonly UserDbContext _context = context;

    /// <summary>
    /// Obtiene un usuario por correo electrónico cargando su colección de películas en carrito
    /// </summary>
    /// <param name="email">Correo electrónico del usuario a recuperar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>El usuario con su carrito si existe; en caso contrario, <see langword="null"/>.</returns>
    public async Task<AppUser?> GetUserWithCartByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.AppUsers.Include(user => user.CartMovies)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    /// <summary>
    /// Guarda en la base de datos los cambios pendientes del contexto de usuarios
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la finalización del guardado.</returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
