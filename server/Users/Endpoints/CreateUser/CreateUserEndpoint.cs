using Contracts.Core;
using Microsoft.AspNetCore.Identity;
using Users.Data.Domain;

namespace Users.Endpoints.CreateUser;

/// <summary>
/// Solicitud para crear un nuevo usuario con sus datos de registro.
/// </summary>
/// <param name="FullName">Nombre completo del usuario.</param>
/// <param name="Email">Correo electrónico que se usará como identificador de acceso.</param>
/// <param name="Password">Contraseña inicial de la cuenta.</param>
public sealed record CreateUserRequest(string FullName, string Email, string Password);

/// <summary>
/// Valida que la solicitud <see cref="CreateUserRequest"/> contenga los datos obligatorios de registro.
/// </summary>
public sealed class CreateUserRequestValidator : IValidator<CreateUserRequest>
{
    /// <summary>
    /// Ejecuta las reglas de validación para la creación de usuario.
    /// </summary>
    /// <param name="request">Solicitud con los datos a validar.</param>
    /// <returns>Secuencia de errores de validación encontrados.</returns>
    public IEnumerable<ValidationError> Validate(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            yield return new ValidationError(nameof(request.Email), "El email es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            yield return new ValidationError(nameof(request.Password), "La contraseña es obligatoria.");
        }
    }
}

/// <summary>
/// Endpoint que crea un usuario cuando la solicitud <see cref="CreateUserRequest"/> es válida.
/// </summary>
internal class CreateUserEndpoint(UserManager<AppUser> userManager) : ValidatedEndpoint<CreateUserRequest>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    /// <summary>
    /// Crea el usuario en el almacén de identidad a partir de una solicitud validada.
    /// </summary>
    /// <param name="request">Datos validados para el alta de usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    protected override async Task OnValidatedAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email
        };

        await _userManager.CreateAsync(newUser, request.Password);

        await Send.OkAsync();
    }
}
