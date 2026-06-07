namespace Mediator.Contracts;

/// <summary>
/// Define el contrato de un mediador encargado de enviar solicitudes a sus controladores.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Envía una solicitud y devuelve una respuesta del tipo especificado.
    /// </summary>
    /// <typeparam name="TResponse">Tipo de respuesta asociado a la solicitud.</typeparam>
    /// <param name="request">Solicitud a procesar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la respuesta.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}