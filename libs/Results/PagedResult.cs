namespace Results;

/// <summary>
/// Representa un resultado exitoso o fallido que además incluye metadatos de paginación.
/// </summary>
/// <typeparam name="T">Tipo del valor contenido en el resultado.</typeparam>
public class PagedResult<T> : Result<T>
{
    /// <summary>
    /// Obtiene la información de paginación asociada al resultado.
    /// </summary>
    public PagedInfo PagedInfo { get; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="PagedResult{T}"/>.
    /// </summary>
    /// <param name="pagedInfo">Metadatos de paginación.</param>
    /// <param name="value">Valor contenido en el resultado.</param>
    public PagedResult(PagedInfo pagedInfo, T value) : base(value)
    {
        PagedInfo = pagedInfo;
    }
}