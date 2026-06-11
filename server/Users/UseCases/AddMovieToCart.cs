using Contracts.MovieDetails;
using Mediator.Contracts;
using Results;
using Users.Data.Domain;
using Users.Data.Repositories;

namespace Users.UseCases;

/// <summary>
/// Comando para agregar una película al carrito del usuario autenticado
/// </summary>
/// <param name="MovieId">Identificador de la película que se desea agregar al carrito.</param>
/// <param name="Quantity">Cantidad de unidades a agregar.</param>
/// <param name="EmailAddress">Correo electrónico del usuario propietario del carrito.</param>
public record AddMovieToCartCommand(Guid MovieId, int Quantity, string EmailAddress) : IRequest<Result>;

/// <summary>
/// Manejador de <see cref="AddMovieToCartCommand"/> que valida la existencia del usuario y la película
/// antes de persistir el nuevo elemento del carrito
/// </summary>
/// <param name="appUserRepository">Repositorio para consultar y persistir el usuario y su carrito.</param>
/// <param name="mediator">Mediador utilizado para consultar detalles de la película.</param>
internal class AddMovieToCartHandler(IAppUserRepository appUserRepository, IMediator mediator) : IRequestHandler<AddMovieToCartCommand, Result>
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Procesa el comando de agregado al carrito verificando usuario y disponibilidad de la película
    /// </summary>
    /// <param name="request">Comando con los datos de la película, cantidad y usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un resultado exitoso si la película se agrega correctamente; de lo contrario, un resultado
    /// <see cref="ResultStatus.Unauthorized"/> o <see cref="ResultStatus.NotFound"/> según corresponda.
    /// </returns>
    public async Task<Result> Handle(AddMovieToCartCommand request, CancellationToken cancellationToken)
    {
        var user = await _appUserRepository.GetUserWithCartByEmailAsync(request.EmailAddress, cancellationToken);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var query = new MovieDetailsQuery(request.MovieId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            return Result.NotFound();
        }

        var movieDetails = result.Value;

        var newCartItem = new CartMovie(movieDetails!.MovieId, movieDetails.Title, request.Quantity, movieDetails.Price);

        user.AddMovieToCart(newCartItem);

        await _appUserRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
