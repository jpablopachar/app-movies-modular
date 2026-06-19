using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Users.Data.Domain;

namespace Users.Endpoints.Login;

/// <summary>
/// Solicitud de autenticación con credenciales de usuario
/// </summary>
/// <param name="Email">Correo electrónico del usuario registrado</param>
/// <param name="Password">Contraseña asociada a la cuenta</param>
public sealed record LoginRequest(string Email, string Password);

/// <summary>
/// Endpoint que autentica un usuario y devuelve un token JWT cuando las credenciales son válidas
/// </summary>
internal class LoginEndpoint(UserManager<AppUser> userManager) : Endpoint<LoginRequest>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    /// <summary>
    /// Configura la ruta HTTP y permite acceso anónimo para iniciar sesión
    /// </summary>
    public override void Configure()
    {
        Post("/api/users/login");
        AllowAnonymous();
    }

    /// <summary>
    /// Procesa la autenticación por correo y contraseña y responde con un token JWT o estado no autorizado
    /// </summary>
    /// <param name="request">Datos de autenticación enviados por el cliente</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica</param>
    /// <returns>Tarea asincrónica que representa el envío de la respuesta HTTP</returns>
    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            await Send.UnauthorizedAsync(cancellationToken);

            return;
        }

        var jwtSecret = Config["Auth:JwtSecret"]!;

        var token = JwtBearer.CreateToken(option =>
        {
            option.SigningKey = jwtSecret;
            option.ExpireAt = DateTime.UtcNow.AddHours(500);
            option.User["sub"] = user.Id;
            option.User["email"] = user.Email!;
            option.User["name"] = user.FullName!;
            option.User["EmailAddress"] = user.Email!;
        });

        await Send.OkAsync(token, cancellationToken);
    }
}
