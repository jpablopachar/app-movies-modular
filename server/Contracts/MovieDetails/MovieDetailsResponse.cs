namespace Contracts.MovieDetails;

/// <summary>
/// Representa la respuesta con el detalle de una película.
/// </summary>
/// <param name="MovieId">Identificador único de la película.</param>
/// <param name="Title">Título de la película.</param>
/// <param name="Description">Descripción de la película.</param>
/// <param name="Price">Precio de la película.</param>
public record MovieDetailsResponse
(
    Guid MovieId,
    string Title,
    string Description,
    decimal Price
);
