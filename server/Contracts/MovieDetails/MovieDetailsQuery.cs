using Mediator.Contracts;
using Results;

namespace Contracts.MovieDetails;

/// <summary>
/// Solicitud para obtener el detalle de una película por su identificador.
/// </summary>
/// <param name="MovieId">Identificador único de la película.</param>
public record MovieDetailsQuery(Guid MovieId) : IRequest<Result<MovieDetailsResponse>>;
