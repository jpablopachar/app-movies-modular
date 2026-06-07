namespace Mediator.Contracts;

/// <summary>
/// Define el contrato para manejar una solicitud específica y producir una respuesta.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud que maneja el controlador.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta que devuelve el controlador.</typeparam>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Procesa la solicitud y devuelve una respuesta.
    /// </summary>
    /// <param name="request">Solicitud a procesar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la respuesta.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}