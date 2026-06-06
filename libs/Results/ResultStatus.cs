namespace Results;

/// <summary>
/// Enumeración que representa los posibles estados de resultado de una operación.
/// </summary>
public enum ResultStatus
{
    /// <summary>
    /// La operación se completó exitosamente.
    /// </summary>
    Ok,

    /// <summary>
    /// Ocurrió un error durante la operación.
    /// </summary>
    Error,

    /// <summary>
    /// La operación fue rechazada por falta de permisos.
    /// </summary>
    Forbidden,

    /// <summary>
    /// La operación requiere autenticación.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Los datos proporcionados son inválidos.
    /// </summary>
    Invalid,

    /// <summary>
    /// El recurso solicitado no fue encontrado.
    /// </summary>
    NotFound,

    /// <summary>
    /// Existe un conflicto con el estado actual del recurso.
    /// </summary>
    Conflict,

    /// <summary>
    /// Ocurrió un error crítico durante la operación.
    /// </summary>
    CriticalError,

    /// <summary>
    /// El servicio o recurso no está disponible en este momento.
    /// </summary>
    Unavailable
}