namespace Results;

/// <summary>
/// Define los niveles de severidad aplicables a un error de validación.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>
    /// Indica un error que invalida la operación.
    /// </summary>
    Error,

    /// <summary>
    /// Indica una advertencia que no necesariamente bloquea la operación.
    /// </summary>
    Warning,

    /// <summary>
    /// Indica un mensaje informativo relacionado con la validación.
    /// </summary>
    Info
}