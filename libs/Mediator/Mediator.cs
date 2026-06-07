using Mediator.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator;

/// <summary>
/// Implementación del mediador que enruta solicitudes a sus controladores
/// y ejecuta comportamientos de canalización registrados.
/// </summary>
/// <param name="serviceProvider">Proveedor de servicios para resolver controladores y comportamientos.</param>
public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    /// <summary>
    /// Envía una solicitud al controlador correspondiente y ejecuta la cadena
    /// de comportamientos de canalización antes y después del controlador final.
    /// </summary>
    /// <typeparam name="TResponse">Tipo de respuesta esperado para la solicitud.</typeparam>
    /// <param name="request">Solicitud que será procesada.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la respuesta procesada.</returns>
    /// <exception cref="ArgumentNullException">Se produce cuando <paramref name="request"/> es <see langword="null"/>.</exception>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
        {
            var handleMethod = handlerType.GetMethod("Handle")!;
            var taskObj = handleMethod.Invoke(handler, [request, cancellationToken])!;

            return (Task<TResponse>)taskObj;
        };

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType).Reverse().ToList();

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;

            handlerDelegate = () =>
            {
                var handleMethod = behaviorType.GetMethod("Handle")!;
                var taskObj = handleMethod.Invoke(behavior, [request, cancellationToken, next])!;

                return (Task<TResponse>)taskObj;
            };
        }

        return await handlerDelegate();
    }
}