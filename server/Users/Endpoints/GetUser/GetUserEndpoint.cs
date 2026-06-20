using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Users.Data.Domain;

namespace Users.Endpoints.GetUser;

/// <summary>
/// Respuesta con los datos del usuario autenticado.
/// </summary>
/// <param name="Id">Identificador único del usuario.</param>
/// <param name="FullName">Nombre completo del usuario.</param>
/// <param name="Email">Correo electrónico del usuario.</param>
/// <param name="Username">Nombre de usuario asociado a la cuenta.</param>
public sealed record UserResponse(string Id, string FullName, string Email, string Username);

/// <summary>
/// Endpoint que obtiene la información del usuario autenticado y devuelve un <see cref="UserResponse"/>.
/// </summary>
internal class GetUserEndpoint(UserManager<AppUser> userManager) : EndpointWithoutRequest<UserResponse>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    /// <summary>
    /// Configura la ruta y el esquema de autenticación requerido para consultar el usuario actual.
    /// </summary>
    public override void Configure()
    {
        Get("/api/users/me");
        AuthSchemes("Bearer");
    }

    /// <summary>
    /// Obtiene el usuario autenticado desde los claims y devuelve sus datos de perfil.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            await Send.UnauthorizedAsync(cancellationToken);

            return;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            await Send.UnauthorizedAsync(cancellationToken);

            return;
        }

        var response = new UserResponse(user.Id, user.FullName, user.Email!, user.UserName!);

        await Send.OkAsync(response, cancellationToken);
    }
}
