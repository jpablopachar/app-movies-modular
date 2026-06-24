namespace Movies.Data.Domain;

/// <summary>
/// Entidad de dominio que representa a una persona vinculada a producciones cinematográficas
/// </summary>
internal class Person
{
    /// <summary>
    /// Identificador único de la persona
    /// </summary>
    public Guid? PersonId { get; private set; } = Guid.CreateVersion7();

    /// <summary>
    /// Nombre completo con el que se identifica a la persona
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Fecha de nacimiento registrada
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// Biografía o reseña descriptiva
    /// </summary>
    public string? Bio { get; private set; }

    /// <summary>
    /// Participaciones de la persona como parte del reparto
    /// </summary>
    public ICollection<MovieCast>? Casts { get; } = [];

    /// <summary>
    /// Participaciones de la persona como parte del equipo técnico
    /// </summary>
    public ICollection<MovieCrew>? Crews { get; } = [];

    /// <summary>
    /// Inicializa una persona con sus datos biográficos básicos
    /// </summary>
    /// <param name="personId">Identificador de la persona.</param>
    /// <param name="name">Nombre de la persona.</param>
    /// <param name="birthDate">Fecha de nacimiento.</param>
    /// <param name="bio">Biografía o <see langword="null"/> si no se informa.</param>
    internal Person(Guid personId, string name, DateTime birthDate, string? bio)
    {
        PersonId = personId;
        Name = name;
        BirthDate = birthDate;
        Bio = bio;
    }
}
