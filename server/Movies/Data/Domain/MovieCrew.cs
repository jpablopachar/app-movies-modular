namespace Movies.Data.Domain;

/// <summary>
/// Entidad de unión que representa la participación técnica de una persona en una película
/// </summary>
internal class MovieCrew
{
    /// <summary>
    /// Identificador de la película asociada
    /// </summary>
    public Guid? MovieId { get; private set; }

    /// <summary>
    /// Identificador de la persona del equipo técnico
    /// </summary>
    public Guid? PersonId { get; private set; }

    /// <summary>
    /// Rol desempeñado en la producción
    /// </summary>
    public string? Role { get; private set; }

    /// <summary>
    /// Navegación hacia la película relacionada
    /// </summary>
    public Movie? Movie { get; private set; }

    /// <summary>
    /// Navegación hacia la persona relacionada
    /// </summary>
    public Person? Person { get; private set; }

    /// <summary>
    /// Crea una relación de equipo técnico entre una película y una persona
    /// </summary>
    /// <param name="movieId">Identificador de la película.</param>
    /// <param name="personId">Identificador de la persona.</param>
    /// <param name="role">Rol técnico desempeñado.</param>
    internal MovieCrew(Guid movieId, Guid personId, string role)
    {
        MovieId = movieId;
        PersonId = personId;
        Role = role;
    }
}
