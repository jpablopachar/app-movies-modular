namespace Results;

/// <summary>
/// Define el contrato base para representar el resultado de una operación.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Obtiene el estado final de la operación.
    /// </summary>
    ResultStatus Status { get; }

    /// <summary>
    /// Obtiene la colección de errores asociados al resultado.
    /// </summary>
    IEnumerable<string> Errors { get; }

    /// <summary>
    /// Obtiene la lista de errores de validación detallados.
    /// </summary>
    List<ResultValidationError> ValidationErrors { get; }

    /// <summary>
    /// Obtiene el tipo del valor contenido en el resultado.
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    /// Obtiene el valor encapsulado como <see cref="object"/>.
    /// </summary>
    /// <returns>El valor contenido en el resultado.</returns>
    object GetValue();
}