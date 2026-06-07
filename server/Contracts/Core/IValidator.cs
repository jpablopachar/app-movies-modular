namespace Contracts.Core;

/// <summary>
/// Define la validación de una solicitud de tipo <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Tipo de la solicitud que se valida.</typeparam>
public interface IValidator<TRequest>
{
    /// <summary>
    /// Ejecuta la validación de la solicitud indicada y devuelve los errores encontrados.
    /// </summary>
    /// <param name="request">Solicitud que se va a validar.</param>
    /// <returns>Colección de errores de validación detectados.</returns>
    IEnumerable<ValidationError> Validate(TRequest request);
}
