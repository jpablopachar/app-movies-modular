namespace Movies.Data.Domain;

/// <summary>
/// Entidad de unión que representa la participación de una persona en el reparto de una película
/// </summary>
internal class MovieCast
{
    /// <summary>
    /// Identificador de la película asociada
    /// </summary>
    public Guid? MovieId { get; private set; }

    /// <summary>
    /// Identificador de la persona del reparto
    /// </summary>
    public Guid? PersonId { get; private set; }

    /// <summary>
    /// Nombre del personaje interpretado
    /// </summary>
    public string? CharacterName { get; private set; }

    /// <summary>
    /// Orden de aparición del elenco en créditos
    /// </summary>
    public int CastOrder { get; private set; }

    /// <summary>
    /// Navegación hacia la película relacionada
    /// </summary>
    public Movie? Movie { get; private set; }

    /// <summary>
    /// Navegación hacia la persona relacionada
    /// </summary>
    public Person? Person { get; private set; }

    /// <summary>
    /// Crea una relación de reparto entre una película y una persona
    /// </summary>
    /// <param name="movieId">Identificador de la película.</param>
    /// <param name="personId">Identificador de la persona.</param>
    /// <param name="characterName">Nombre del personaje interpretado.</param>
    /// <param name="castOrder">Orden de créditos dentro del elenco.</param>
    internal MovieCast(Guid movieId, Guid personId, string characterName, int castOrder)
    {
        MovieId = movieId;
        PersonId = personId;
        CharacterName = characterName;
        CastOrder = castOrder;
    }
}
