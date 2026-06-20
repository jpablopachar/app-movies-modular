using System.Security.Claims;
using FastEndpoints;
using Mediator.Contracts;
using Results;
using Users.UseCases;

namespace Users.Endpoints.AddMovie;

/// <summary>
/// Solicitud para agregar una película al carrito del usuario autenticado.
/// </summary>
/// <param name="MovieId">Identificador de la película que se desea agregar.</param>
/// <param name="Quantity">Cantidad de unidades a agregar al carrito.</param>
public record AddCartMovieRequest(Guid MovieId, int Quantity);

/// <summary>
/// Endpoint que agrega una película al carrito del usuario a partir de <see cref="AddCartMovieRequest"/>.
/// </summary>
internal class AddMovieEndpoint(IMediator mediator) : Endpoint<AddCartMovieRequest>
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Configura la ruta y los requisitos de claims para el endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("/api/cart");
        Claims("EmailAddress");
    }

    /// <summary>
    /// Procesa la solicitud y envía el comando para agregar una película al carrito.
    /// </summary>
    /// <param name="request">Datos de la película y cantidad a registrar en el carrito.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    public override async Task HandleAsync(AddCartMovieRequest request, CancellationToken cancellationToken)
    {
        var emailAddress = User.FindFirstValue("EmailAddress");

        var command = new AddMovieToCartCommand(request.MovieId, request.Quantity, emailAddress!);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await Send.UnauthorizedAsync(cancellationToken);
        }

        await Send.OkAsync(cancellationToken);
    }
}
