using MediaBizzare.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaBizzare.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly CartService _cart;

        public CartController(CartService cart) => _cart = cart;

        // POST /Cart/Add   body: { "variationId": 3, "quantity": 1 }
        [HttpPost("Add")]
        public IActionResult Add([FromBody] AddToCartRequest req)
        {
            if (req.VariationId <= 0)
                return BadRequest(new { error = "Invalid variation" });

            _cart.Add(req.VariationId, req.Quantity > 0 ? req.Quantity : 1);
            return Ok(new { count = _cart.Count() });
        }

        // POST /Cart/Remove   body: { "variationId": 3 }
        [HttpPost("Remove")]
        public IActionResult Remove([FromBody] VariationIdRequest req)
        {
            _cart.Remove(req.VariationId);
            return Ok(new { count = _cart.Count() });
        }

        // POST /Cart/Update   body: { "variationId": 3, "quantity": 2 }
        [HttpPost("Update")]
        public IActionResult Update([FromBody] UpdateCartRequest req)
        {
            _cart.Update(req.VariationId, req.Quantity);
            return Ok(new { count = _cart.Count() });
        }

        // GET /Cart/Count
        [HttpGet("Count")]
        public IActionResult Count() => Ok(new { count = _cart.Count() });
    }

    // ── Request DTOs ───────────────────────────────────────────────────────

    public class AddToCartRequest
    {
        public int VariationId { get; set; }
        public int Quantity    { get; set; } = 1;
    }

    public class VariationIdRequest
    {
        public int VariationId { get; set; }
    }

    public class UpdateCartRequest
    {
        public int VariationId { get; set; }
        public int Quantity    { get; set; }
    }
}
