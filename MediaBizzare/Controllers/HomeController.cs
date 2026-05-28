using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MediaBizzare.Data;
using MediaBizzare.Models;
using MediaBizzare.Models.ViewModels;
using MediaBizzare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly CartService _cart;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, CartService cart)
        {
            _logger = logger;
            _context = context;
            _cart = cart;
        }

        // ---------- HOME ----------

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variations)
                .ToList();

            HomeIndexVM vm = new HomeIndexVM();
            vm.Categories = BuildCategoryTiles();
            vm.Bestsellers = ToProductCards(products).Take(8).ToList();

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Old route kept so the existing nav link doesn't 404.
        public IActionResult HomePage()
        {
            return RedirectToAction("Index");
        }

        // ---------- CATEGORY LANDING ----------

        public IActionResult Categories()
        {
            HomeIndexVM vm = new HomeIndexVM();
            vm.Categories = BuildCategoryTiles();
            return View(vm);
        }

        // ---------- CATEGORY PAGE (dynamic — driven by the DB slug) ----------

        public IActionResult Category(string slug)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Variations)
                .FirstOrDefault(c => c.Slug == slug);

            if (category == null)
                return NotFound();

            CategoryPageVM vm = new CategoryPageVM();
            vm.CategoryName = category.Name;
            vm.Action = slug;
            vm.Products = ToProductCards(category.Products);
            vm.Filters = new List<string>();

            return View(vm);
        }

        // ---------- PRODUCT DETAIL ----------

        public IActionResult Product(int id = 1)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variations)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var cheapest = product.Variations.OrderBy(v => v.Price).FirstOrDefault();

            ProductDetailVM vm = new ProductDetailVM();
            vm.Id = product.Id;
            vm.Name = product.Name;
            vm.CategoryName = product.Category?.Name ?? "";
            vm.CategoryAction = "Categories";
            vm.Price = cheapest?.Price ?? 0m;
            vm.Description = product.Description;
            vm.Images = product.Variations
                .Where(v => !string.IsNullOrWhiteSpace(v.ImageUrl))
                .Select(v => v.ImageUrl!)
                .Distinct()
                .ToList();
            vm.Variants = product.Variations
                .OrderBy(v => v.Price)
                .Select(v => new VariantOptionVM
                {
                    VariationId = v.Id,
                    Label       = v.SKU,
                    Price       = v.Price
                })
                .ToList();

            return View(vm);
        }

        // ---------- CART ----------

        public IActionResult Cart()
        {
            var sessionItems = _cart.GetItems();
            var vm = new CartVM();

            if (!sessionItems.Any())
                return View(vm);

            var variationIds = sessionItems.Select(i => i.VariationId).ToList();
            var variations = _context.ProductVariations
                .Include(v => v.Product)
                .Where(v => variationIds.Contains(v.Id))
                .ToList();

            foreach (var si in sessionItems)
            {
                var variation = variations.FirstOrDefault(v => v.Id == si.VariationId);
                if (variation == null) continue;

                vm.Items.Add(new CartItemVM
                {
                    Id          = variation.Product?.Id ?? 0,
                    VariationId = variation.Id,
                    Name        = variation.Product?.Name ?? "",
                    ImageUrl    = variation.ImageUrl ?? "",
                    Variant     = variation.SKU,
                    UnitPrice   = variation.Price,
                    Quantity    = si.Quantity,
                    LineTotal   = variation.Price * si.Quantity
                });
            }

            vm.Subtotal = vm.Items.Sum(i => i.LineTotal);
            vm.Shipping = (vm.Subtotal == 0m || vm.Subtotal >= 50m) ? 0m : 4.95m;
            vm.Total    = vm.Subtotal + vm.Shipping;

            return View(vm);
        }

        // ---------- ERROR ----------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string requestId;
            if (Activity.Current != null && Activity.Current.Id != null)
            {
                requestId = Activity.Current.Id;
            }
            else
            {
                requestId = HttpContext.TraceIdentifier;
            }
            return View(new ErrorViewModel { RequestId = requestId });
        }

        // ---------- HELPERS ----------

        private List<ProductCardVM> ToProductCards(IEnumerable<Product> products)
        {
            var result = new List<ProductCardVM>();
            foreach (var p in products)
            {
                decimal price = p.Variations.Any() ? p.Variations.Min(v => v.Price) : 0m;
                ProductCardVM card = new ProductCardVM();
                card.Id = p.Id;
                card.Slug = p.Slug;
                card.Name = p.Name;
                card.Price = price;
                card.Category = p.Category?.Name ?? "";
                // Use the first variation that has an image URL
                var cheapest = p.Variations.OrderBy(v => v.Price).FirstOrDefault();
                card.DefaultVariationId = cheapest?.Id ?? 0;
                card.ImageUrl = p.Variations
                    .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v.ImageUrl))?.ImageUrl ?? "";
                result.Add(card);
            }
            return result;
        }

        private List<CategoryTileVM> BuildCategoryTiles()
        {
            return _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryTileVM
                {
                    Name = c.Name,
                    Slug = c.Slug,
                    Action = "Category",
                    Span = 1
                })
                .ToList();
        }
    }
}
