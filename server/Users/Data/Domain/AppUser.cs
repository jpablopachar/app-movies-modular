using Microsoft.AspNetCore.Identity;

namespace Users.Data.Domain;

/// <summary>
/// Representa un usuario de la aplicación con soporte para gestionar su carrito de películas.
/// </summary>
public sealed class AppUser : IdentityUser
{
    /// <summary>
    /// Obtiene o establece el nombre completo del usuario.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    private readonly List<CartMovie> _cartMovies = [];

    /// <summary>
    /// Obtiene la colección de películas agregadas al carrito del usuario.
    /// </summary>
    public IReadOnlyCollection<CartMovie> CartMovies => _cartMovies.AsReadOnly();

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AppUser"/>.
    /// </summary>
    public AppUser() { }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AppUser"/> con el nombre de usuario y el nombre completo.
    /// </summary>
    /// <param name="userName">Nombre de usuario asociado a la cuenta.</param>
    /// <param name="fullName">Nombre completo del usuario.</param>
    public AppUser(string userName, string fullName) : base(userName) { }

    /// <summary>
    /// Elimina todas las películas del carrito del usuario.
    /// </summary>
    public void ClearCart() => _cartMovies.Clear();

    /// <summary>
    /// Actualiza el nombre completo del usuario.
    /// </summary>
    /// <param name="fullName">Nuevo nombre completo.</param>
    /// <exception cref="ArgumentException">Se produce cuando el nombre completo está vacío o solo contiene espacios.</exception>
    internal void UpdateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo no puede estar vacío.", nameof(fullName));

        FullName = fullName;
    }

    /// <summary>
    /// Agrega una película al carrito o actualiza sus datos si ya existe.
    /// </summary>
    /// <param name="item">Elemento que se desea agregar al carrito.</param>
    /// <returns>La película agregada o actualizada dentro del carrito.</returns>
    /// <exception cref="ArgumentNullException">Se produce cuando <paramref name="item"/> es <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Se produce cuando la cantidad total resultante es cero o negativa.</exception>
    internal CartMovie AddMovieToCart(CartMovie item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        var itemFromCart = _cartMovies.FirstOrDefault(m => m.MovieId == item.MovieId);

        if (itemFromCart is not null)
        {
            var newQuantity = itemFromCart.Quantity + item.Quantity;

            if (newQuantity <= 0) throw new InvalidOperationException("La cantidad total de un artículo en el carrito no puede ser cero o negativa.");

            itemFromCart.UpdateQuantity(newQuantity);
            itemFromCart.UpdateDescription(item.Description);
            itemFromCart.UpdateUnitPrice(item.UnitPrice);

            return itemFromCart;
        }

        _cartMovies.Add(item);

        return item;
    }

    /// <summary>
    /// Quita una película del carrito si existe un elemento asociado al identificador indicado.
    /// </summary>
    /// <param name="movieId">Identificador de la película que se desea eliminar.</param>
    internal void RemoveItemFromCart(Guid movieId)
    {
        var item = _cartMovies.FirstOrDefault(m => m.MovieId == movieId);

        if (item is null) return;

        _cartMovies.Remove(item);
    }
}
