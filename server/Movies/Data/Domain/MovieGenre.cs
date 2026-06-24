namespace Movies.Data.Domain;

/// <summary>
/// Entidad de unión que relaciona una película con un género
/// </summary>
internal class MovieGenre
{
    /// <summary>
    /// Identificador de la película asociada
    /// </summary>
    public Guid? MovieId { get; private set; }

    /// <summary>
    /// Identificador del género asociado
    /// </summary>
    public Guid? GenreId { get; private set; }

    /// <summary>
    /// Navegación hacia la película relacionada
    /// </summary>
    public Movie? Movie { get; private set; }

    /// <summary>
    /// Navegación hacia el género relacionado
    /// </summary>
    public Genre? Genre { get; private set; }

    /// <summary>
    /// Crea una relación entre una película y un género
    /// </summary>
    /// <param name="movieId">Identificador de la película.</param>
    /// <param name="genreId">Identificador del género.</param>
    internal MovieGenre(Guid movieId, Guid genreId)
    {
        MovieId = movieId;
        GenreId = genreId;
    }
}
