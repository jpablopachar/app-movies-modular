namespace Users.Data.Domain;

public sealed class CartMovie
{
    public string UserId { get; private set; } = default!;
    public AppUser User { get; private set; } = default!;
    public Guid MovieId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public CartMovie(Guid movieId, string description, int quantity, decimal unitPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        MovieId = movieId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    internal void UpdateQuantity(int newQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newQuantity);

        Quantity = newQuantity;
    }

    internal void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    internal void UpdateUnitPrice(decimal newPrice)
    {
        UnitPrice = newPrice;
    }
}
