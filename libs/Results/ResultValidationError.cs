namespace Results;

/// <summary>
/// Representa un error de validación con información estructurada.
/// </summary>
public class ResultValidationError
{
    /// <summary>
    /// Obtiene el identificador del campo o elemento que produjo el error.
    /// </summary>
    public string? Identifier { get; private set; }

    /// <summary>
    /// Obtiene el mensaje descriptivo del error.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Obtiene el código asociado al error de validación.
    /// </summary>
    public string? ErrorCode { get; private set; }

    /// <summary>
    /// Obtiene la severidad del error de validación.
    /// </summary>
    public ValidationSeverity Severity { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia vacía de <see cref="ResultValidationError"/>.
    /// </summary>
    public ResultValidationError()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia con un mensaje de error.
    /// </summary>
    /// <param name="errorMessage">Mensaje descriptivo del error.</param>
    public ResultValidationError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Inicializa una nueva instancia con todos los detalles del error de validación.
    /// </summary>
    /// <param name="identifier">Identificador del campo o elemento validado.</param>
    /// <param name="errorMessage">Mensaje descriptivo del error.</param>
    /// <param name="errorCode">Código asociado al error.</param>
    /// <param name="severity">Nivel de severidad del error.</param>
    public ResultValidationError(string identifier, string errorMessage, string errorCode,
        ValidationSeverity severity = ValidationSeverity.Error)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        Severity = severity;
    }
}