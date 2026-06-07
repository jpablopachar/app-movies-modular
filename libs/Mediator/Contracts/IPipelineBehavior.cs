namespace Mediator.Contracts;

/// <summary>
/// Representa el siguiente paso en la cadena de ejecución de la solicitud.
/// </summary>
/// <typeparam name="TResponse">Tipo de respuesta que devuelve la cadena.</typeparam>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Define un comportamiento de canalización que envuelve la ejecución de un controlador.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud procesada por la canalización.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta generada por la canalización.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Ejecuta lógica antes y/o después del siguiente elemento de la cadena.
    /// </summary>
    /// <param name="request">Solicitud actual.</param>
    /// <param name="next">Delegado que invoca el siguiente paso de la canalización.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la respuesta.</returns>
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}