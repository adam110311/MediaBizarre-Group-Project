using System.Text.Json;
using MediaBizzare.Models;

namespace MediaBizzare.Services
{
    public class CartService
    {
        private const string SessionKey = "MediaBazar_Cart";
        private readonly IHttpContextAccessor _http;

        public CartService(IHttpContextAccessor http) => _http = http;

        // ── Read ────────────────────────────────────────────────────────────

        public List<CartSessionItem> GetItems()
        {
            var session = _http.HttpContext?.Session;
            var json = session?.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
                return new List<CartSessionItem>();

            return JsonSerializer.Deserialize<List<CartSessionItem>>(json)
                   ?? new List<CartSessionItem>();
        }

        public int Count() => GetItems().Sum(i => i.Quantity);

        // ── Write ───────────────────────────────────────────────────────────

        public void Add(int variationId, int quantity = 1)
        {
            var items = GetItems();
            var existing = items.FirstOrDefault(i => i.VariationId == variationId);

            if (existing != null)
                existing.Quantity += quantity;
            else
                items.Add(new CartSessionItem { VariationId = variationId, Quantity = quantity });

            Save(items);
        }

        public void Remove(int variationId)
        {
            var items = GetItems();
            items.RemoveAll(i => i.VariationId == variationId);
            Save(items);
        }

        public void Update(int variationId, int quantity)
        {
            var items = GetItems();
            var existing = items.FirstOrDefault(i => i.VariationId == variationId);

            if (existing == null) return;

            if (quantity <= 0)
                items.Remove(existing);
            else
                existing.Quantity = quantity;

            Save(items);
        }

        public void Clear()
        {
            _http.HttpContext?.Session.Remove(SessionKey);
        }

        // ── Private ─────────────────────────────────────────────────────────

        private void Save(List<CartSessionItem> items)
        {
            _http.HttpContext?.Session.SetString(SessionKey, JsonSerializer.Serialize(items));
        }
    }
}
