namespace MediaBizzare.Models
{
    /// <summary>
    /// Lightweight item stored in the session cart.
    /// Only stores the variation ID and quantity — all other
    /// data (name, price, image) is loaded from the DB on demand.
    /// </summary>
    public class CartSessionItem
    {
        public int VariationId { get; set; }
        public int Quantity    { get; set; }
    }
}
