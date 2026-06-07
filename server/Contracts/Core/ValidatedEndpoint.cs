using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Core;

/// <summary>
/// Proporciona una base para endpoints que validan la solicitud antes de procesarla.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud que recibe el endpoint.</typeparam>
public abstract class ValidatedEndpoint<TRequest> : Endpoint<TRequest> where TRequest : notnull
{
    /// <summary>
    /// Ejecuta la validación registrada para la solicitud y, si no hay errores, continúa con el procesamiento.
    /// </summary>
    /// <param name="req">Solicitud recibida por el endpoint.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    public sealed override async Task HandleAsync(TRequest req, CancellationToken cancellationToken)
    {
        var validators = HttpContext.RequestServices.GetServices<IValidator<TRequest>>();

        var errors = validators.SelectMany(v => v.Validate(req)).ToList();

        if (errors.Count != 0)
        {
            var dict = errors
                .GroupBy(e => ToCamel(e.PropertyName))
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var payload = new
            {
                statusCode = 400,
                message = "Validation failed",
                errors = dict
            };

            HttpContext.Response.StatusCode = 400;

            await HttpContext.Response.WriteAsJsonAsync(payload, cancellationToken);

            return;
        }

        await OnValidatedAsync(req, cancellationToken);
    }

    /// <summary>
    /// Continúa con la lógica del endpoint cuando la validación ha sido correcta.
    /// </summary>
    /// <param name="req">Solicitud validada.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    protected abstract Task OnValidatedAsync(TRequest req, CancellationToken cancellationToken);

    private static string ToCamel(string s) => string.IsNullOrEmpty(s) || char.IsLower(s[0]) ? s : char.ToLowerInvariant(s[0]) + s[1..];
}
