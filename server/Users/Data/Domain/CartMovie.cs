namespace Users.Data.Domain;

/// <summary>
/// Representa una película agregada al carrito de un usuario.
/// </summary>
public sealed class CartMovie
{
    /// <summary>
    /// Obtiene el identificador del usuario propietario del carrito.
    /// </summary>
    public string UserId { get; private set; } = default!;

    /// <summary>
    /// Obtiene la referencia al usuario propietario del carrito.
    /// </summary>
    public AppUser User { get; private set; } = default!;

    /// <summary>
    /// Obtiene el identificador de la película.
    /// </summary>
    public Guid MovieId { get; private set; }

    /// <summary>
    /// Obtiene la descripción de la película en el momento de agregarla al carrito.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Obtiene la cantidad de unidades agregadas al carrito.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Obtiene el precio unitario de la película en el carrito.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CartMovie"/>.
    /// </summary>
    /// <param name="movieId">Identificador de la película.</param>
    /// <param name="description">Descripción de la película.</param>
    /// <param name="quantity">Cantidad de unidades.</param>
    /// <param name="unitPrice">Precio unitario de la película.</param>
    /// <exception cref="ArgumentOutOfRangeException">Se produce cuando <paramref name="quantity"/> es cero o negativo.</exception>
    public CartMovie(Guid movieId, string description, int quantity, decimal unitPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        MovieId = movieId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    /// <summary>
    /// Actualiza la cantidad de unidades del elemento en el carrito.
    /// </summary>
    /// <param name="newQuantity">Nueva cantidad de unidades.</param>
    /// <exception cref="ArgumentOutOfRangeException">Se produce cuando <paramref name="newQuantity"/> es cero o negativo.</exception>
    internal void UpdateQuantity(int newQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newQuantity);

        Quantity = newQuantity;
    }

    /// <summary>
    /// Actualiza la descripción de la película almacenada en el carrito.
    /// </summary>
    /// <param name="newDescription">Nueva descripción de la película.</param>
    internal void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    /// <summary>
    /// Actualiza el precio unitario de la película almacenada en el carrito.
    /// </summary>
    /// <param name="newPrice">Nuevo precio unitario.</param>
    internal void UpdateUnitPrice(decimal newPrice)
    {
        UnitPrice = newPrice;
    }
}
