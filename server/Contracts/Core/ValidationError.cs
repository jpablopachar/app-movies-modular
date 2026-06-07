namespace Contracts.Core;

/// <summary>
/// Representa un error de validación asociado a una propiedad concreta.
/// </summary>
/// <param name="PropertyName">Nombre de la propiedad con el error.</param>
/// <param name="ErrorMessage">Mensaje descriptivo del error.</param>
public sealed record ValidationError(string PropertyName, string ErrorMessage);
