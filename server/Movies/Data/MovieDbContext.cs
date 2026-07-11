using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Domain;

namespace Movies.Data;

/// <summary>
/// Contexto de Entity Framework Core del módulo Movies.
/// </summary>
/// <param name="options">Opciones de configuración del contexto.</param>
public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Conjunto de entidades de películas.
    /// </summary>
    internal DbSet<Movie> Movies { get; set; }

    /// <summary>
    /// Conjunto de entidades de géneros.
    /// </summary>
    internal DbSet<Genre> Genres { get; set; }

    /// <summary>
    /// Conjunto de entidades de personas.
    /// </summary>
    internal DbSet<Person> People { get; set; }

    /// <summary>
    /// Conjunto de entidades de relación entre películas y géneros.
    /// </summary>
    internal DbSet<MovieGenre> MovieGenres { get; set; }

    /// <summary>
    /// Conjunto de entidades de reparto por película.
    /// </summary>
    internal DbSet<MovieCast> MovieCasts { get; set; }

    /// <summary>
    /// Conjunto de entidades de equipo técnico por película.
    /// </summary>
    internal DbSet<MovieCrew> MovieCrews { get; set; }

    /// <summary>
    /// Configura el modelo de datos, aplica configuraciones y registra datos semilla.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("movies");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        SeedGenres(modelBuilder);
        SeedPeople(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Inserta los géneros iniciales del sistema.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
    private static void SeedGenres(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Action"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Drama"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Comedy"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Sci-Fi"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000005"), "Thriller"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000006"), "Fantasy"),
            new Genre(Guid.Parse("00000000-0000-0000-0000-000000000007"), "Horror")
        );
    }

    /// <summary>
    /// Inserta las personas iniciales del sistema.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
    private static void SeedPeople(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Christopher Nolan",
                Utc(1970, 7, 30),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                "Keany Reeves",
                Utc(1975, 6, 12),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000003"),
                "Carrie-Ann Moss",
                Utc(1980, 1, 15),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000004"),
                "Leonardo DiCaprio",
                Utc(1980, 5, 9),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000005"),
                "Hanz Zimmer",
                Utc(1950, 2, 28),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000006"),
                "Quentin Tarantino",
                Utc(1960, 11, 20),
                null
            ),
            new Person(
                Guid.Parse("00000000-0000-0000-0000-000000000007"),
                "James Cameron",
                Utc(1955, 6, 30),
                null
            )
        );

    }

    /// <summary>
    /// Crea una fecha en formato UTC con hora 00:00:00.
    /// </summary>
    /// <param name="year">Año de la fecha.</param>
    /// <param name="month">Mes de la fecha.</param>
    /// <param name="day">Día de la fecha.</param>
    /// <returns>Instancia de <see cref="DateTime"/> en UTC.</returns>
    private static DateTime Utc(int year, int month, int day)
        => new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Define convenciones globales del modelo para tipos comunes.
    /// </summary>
    /// <param name="configurationBuilder">Constructor de convenciones del modelo.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
