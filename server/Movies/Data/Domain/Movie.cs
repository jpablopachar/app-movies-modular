using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Data.Domain;

/// <summary>
/// Entidad de dominio que representa una película disponible en el catálogo
/// </summary>
internal class Movie
{
    /// <summary>
    /// Identificador único de la película
    /// </summary>
    public Guid? MovieId { get; private set; } = Guid.CreateVersion7();

    /// <summary>
    /// Título principal mostrado en el catálogo
    /// </summary>
    public string? Title { get; private set; } = string.Empty;

    /// <summary>
    /// Título original de estreno de la película
    /// </summary>
    public string? OriginalTitle { get; private set; }

    /// <summary>
    /// Descripción argumental de la película
    /// </summary>
    public string? Synopsis { get; private set; }

    /// <summary>
    /// Fecha de estreno comercial utilizada para clasificar la película
    /// </summary>
    public DateOnly ReleaseYear { get; private set; }

    /// <summary>
    /// Duración total en minutos
    /// </summary>
    public int DurationMinutes { get; private set; }

    /// <summary>
    /// Idioma principal de la pista de audio original
    /// </summary>
    public string? Language { get; private set; }

    /// <summary>
    /// Precio de alquiler vigente de la película
    /// </summary>
    public decimal RentalPrice { get; private set; }

    /// <summary>
    /// Fecha y hora de creación del registro en UTC
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Relación de participaciones del reparto asociadas a la película
    /// </summary>
    public ICollection<MovieCast> Casts { get; } = [];

    /// <summary>
    /// Relación de participaciones del equipo técnico asociadas a la película
    /// </summary>
    public ICollection<MovieCrew> Crews { get; } = [];

    /// <summary>
    /// Relación entre la película y sus géneros
    /// </summary>
    public ICollection<MovieGenre> MovieGenres { get; } = [];

    [NotMapped]
    /// <summary>
    /// Géneros vinculados, excluyendo referencias <see langword="null"/>
    /// </summary>
    public IEnumerable<Genre> Genres => MovieGenres.Select(mg => mg.Genre!).Where(mg => mg is not null);

    [NotMapped]
    /// <summary>
    /// Personas del reparto vinculadas, excluyendo referencias <see langword="null"/>
    /// </summary>
    public IEnumerable<Person> CastPeople => Casts.Select(c => c.Person!).Where(c => c is not null);

    [NotMapped]
    /// <summary>
    /// Personas del equipo técnico vinculadas, excluyendo referencias <see langword="null"/>
    /// </summary>
    public IEnumerable<Person> CrewPeople => Crews.Select(c => c.Person!).Where(c => c is not null);

    /// <summary>
    /// Inicializa una nueva película con sus datos principales y valida invariantes de negocio básicas
    /// </summary>
    /// <param name="title">Título principal de la película.</param>
    /// <param name="releaseYear">Fecha de estreno comercial.</param>
    /// <param name="durationMinutes">Duración total en minutos.</param>
    /// <param name="language">Idioma principal o <see langword="null"/> si no se informa.</param>
    /// <param name="rentalPrice">Precio de alquiler inicial.</param>
    /// <param name="originalTitle">Título original o <see langword="null"/> si coincide con el principal.</param>
    /// <param name="synopsis">Sinopsis o <see langword="null"/> cuando no esté disponible.</param>
    /// <exception cref="ArgumentException">Cuando el título está vacío, la sinopsis supera el límite permitido o el precio es negativo.</exception>
    internal Movie(string title, DateOnly releaseYear, int durationMinutes, string? language, decimal rentalPrice, string? originalTitle = null, string? synopsis = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("El título de la película no puede estar vacío.", nameof(title));
        }

        if (synopsis is not null && synopsis.Length > 4000)
        {
            throw new ArgumentException("La sinopsis no puede exceder los 4000 caracteres.", nameof(synopsis));
        }

        if (rentalPrice < 0m)
        {
            throw new ArgumentException("El precio de alquiler no puede ser negativo.", nameof(rentalPrice));
        }

        Title = title.Trim();
        OriginalTitle = originalTitle?.Trim();
        Synopsis = synopsis?.Trim();
        ReleaseYear = releaseYear;
        DurationMinutes = durationMinutes;
        Language = language?.Trim();
        RentalPrice = rentalPrice;
    }

    /// <summary>
    /// Actualiza el precio de alquiler manteniendo la restricción de no permitir valores negativos
    /// </summary>
    /// <param name="newPrice">Nuevo precio de alquiler que se desea establecer.</param>
    /// <exception cref="ArgumentException">Cuando el nuevo precio es menor que cero.</exception>
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0m)
        {
            throw new ArgumentException("El precio de alquiler no puede ser negativo.", nameof(newPrice));
        }

        if (newPrice == RentalPrice)
        {
            return;
        }

        RentalPrice = newPrice;
    }
}
