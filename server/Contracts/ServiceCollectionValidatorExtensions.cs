using System.Reflection;
using Contracts.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts;

/// <summary>
/// Extensiones para registrar validadores del módulo en el contenedor de inyección de dependencias.
/// </summary>
public static class ServiceCollectionValidatorExtensions
{
    /// <summary>
    /// Registra automáticamente todos los validadores encontrados en el ensamblado indicado.
    /// </summary>
    /// <param name="services">Contenedor de servicios donde se registrarán los validadores.</param>
    /// <param name="assembly">Ensamblado que se va a inspeccionar.</param>
    /// <returns>La misma colección de servicios para permitir el encadenamiento.</returns>
    public static IServiceCollection AddModuleValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
            ).ToList();

        foreach (var validatorType in validatorTypes)
        {
            var validatorInterface = validatorType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

            services.AddScoped(validatorInterface, validatorType);
        }

        return services;
    }
}
