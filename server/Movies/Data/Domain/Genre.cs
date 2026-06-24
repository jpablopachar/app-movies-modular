namespace Movies.Data.Domain;

/// <summary>
/// Entidad de dominio que representa una categoría temática de películas
/// </summary>
internal class Genre
{
    /// <summary>
    /// Identificador único del género
    /// </summary>
    public Guid? GenreId { get; private set; } = Guid.CreateVersion7();

    /// <summary>
    /// Nombre visible del género
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Relación de películas asociadas al género
    /// </summary>
    public ICollection<MovieGenre>? MovieGenres { get; } = [];

    /// <summary>
    /// Inicializa un género con su identificador y nombre
    /// </summary>
    /// <param name="genreId">Identificador del género.</param>
    /// <param name="name">Nombre del género.</param>
    internal Genre(Guid genreId, string name)
    {
        GenreId = genreId;
        Name = name;
    }
}
