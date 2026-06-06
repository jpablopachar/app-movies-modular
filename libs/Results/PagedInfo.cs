namespace Results;

/// <summary>
/// Representa la información de paginación asociada a una colección de resultados.
/// </summary>
/// <param name="pageNumber">Número de página actual.</param>
/// <param name="pageSize">Cantidad de registros por página.</param>
/// <param name="totalPages">Total de páginas disponibles.</param>
/// <param name="totalRecords">Total de registros disponibles.</param>
public class PagedInfo(long pageNumber, long pageSize, long totalPages, long totalRecords)
{
    /// <summary>
    /// Obtiene el número de página actual.
    /// </summary>
    public long PageNumber { get; private set; } = pageNumber;

    /// <summary>
    /// Obtiene el tamaño de página configurado.
    /// </summary>
    public long PageSize { get; private set; } = pageSize;

    /// <summary>
    /// Obtiene el total de páginas disponibles.
    /// </summary>
    public long TotalPages { get; private set; } = totalPages;

    /// <summary>
    /// Obtiene el total de registros disponibles.
    /// </summary>
    public long TotalRecords { get; private set; } = totalRecords;

    /// <summary>
    /// Actualiza el número de página actual.
    /// </summary>
    /// <param name="pageNumber">Nuevo número de página.</param>
    /// <returns>La instancia actual para encadenar llamadas.</returns>
    public PagedInfo SetPageNumber(long pageNumber)
    {
        PageNumber = pageNumber;

        return this;
    }

    /// <summary>
    /// Actualiza el tamaño de página.
    /// </summary>
    /// <param name="pageSize">Nuevo tamaño de página.</param>
    /// <returns>La instancia actual para encadenar llamadas.</returns>
    public PagedInfo SetPageSize(long pageSize)
    {
        PageSize = pageSize;

        return this;
    }

    /// <summary>
    /// Actualiza el total de páginas disponibles.
    /// </summary>
    /// <param name="totalPages">Nuevo total de páginas.</param>
    /// <returns>La instancia actual para encadenar llamadas.</returns>
    public PagedInfo SetTotalPages(long totalPages)
    {
        TotalPages = totalPages;

        return this;
    }

    /// <summary>
    /// Actualiza el total de registros disponibles.
    /// </summary>
    /// <param name="totalRecords">Nuevo total de registros.</param>
    /// <returns>La instancia actual para encadenar llamadas.</returns>
    public PagedInfo SetTotalRecords(long totalRecords)
    {
        TotalRecords = totalRecords;

        return this;
    }
}