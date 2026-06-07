using System.Reflection;
using Mediator.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator;

/// <summary>
/// Proporciona métodos de extensión para registrar el mediador y sus controladores
/// en el contenedor de inyección de dependencias.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra el mediador y descubre automáticamente implementaciones de
    /// <see cref="IRequestHandler{TRequest, TResponse}"/> en los ensamblados indicados.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <param name="assemblies">Ensamblados que se inspeccionarán para encontrar controladores.</param>
    /// <returns>La misma colección de servicios para permitir encadenamiento.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            var concreteTypes = types.Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in concreteTypes)
            {
                var handlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray();

                if (handlerInterfaces.Length == 0) continue;

                if (type.IsGenericTypeDefinition)
                {
                    services.AddScoped(typeof(IRequestHandler<,>), type);
                }
                else
                {
                    foreach (var handlerInterface in handlerInterfaces)
                    {
                        services.AddScoped(handlerInterface, type);
                    }
                }
            }
        }

        return services;
    }
}