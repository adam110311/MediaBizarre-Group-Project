using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MediaBizzare.Data;
using MediaBizzare.Models;
using MediaBizzare.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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

        // ---------- CATEGORY PAGES ----------

        public IActionResult CompLap()
        {
            var products = ProductsBySlug("laptops", "computers", "computer", "laptop");
            var vm = BuildCategoryPage("Computer & laptop", "CompLap",
                new string[] { "Laptops", "Desktops", "Monitors", "Accessories" }, products);
            return View("Category", vm);
        }

        public IActionResult PhoneWear()
        {
            var products = ProductsBySlug("smartphones", "smartphone", "wearables", "wearable", "phones");
            var vm = BuildCategoryPage("Smartphone & wearables", "PhoneWear",
                new string[] { "Smartphones", "Smartwatches", "Earbuds", "Accessories" }, products);
            return View("Category", vm);
        }

        public IActionResult TVaudio()
        {
            var products = ProductsBySlug("tvs", "tv", "audio", "soundbar");
            var vm = BuildCategoryPage("TV & audio", "TVaudio",
                new string[] { "Televisions", "Speakers", "Soundbars", "Headphones" }, products);
            return View("Category", vm);
        }

        public IActionResult HA()
        {
            var products = ProductsBySlug("household-appliances", "household", "kitchen", "laundry", "cleaning");
            var vm = BuildCategoryPage("Household appliances", "HA",
                new string[] { "Kitchen", "Laundry", "Cleaning", "Small appliances" }, products);
            return View("Category", vm);
        }

        public IActionResult GameDivert()
        {
            var products = ProductsBySlug("gaming", "games", "consoles", "entertainment");
            var vm = BuildCategoryPage("Game & entertainment", "GameDivert",
                new string[] { "Consoles", "Games", "Controllers", "VR" }, products);
            return View("Category", vm);
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

            decimal price = product.Variations.Any()
                ? product.Variations.Min(v => v.Price)
                : 0m;

            ProductDetailVM vm = new ProductDetailVM();
            vm.Id = product.Id;
            vm.Name = product.Name;
            vm.CategoryName = product.Category?.Name ?? "";
            vm.CategoryAction = "Categories";
            vm.Price = price;
            vm.Description = product.Description;
            vm.Images = new List<string>();
            vm.Variants = product.Variations.Select(v => v.SKU).ToList();

            return View(vm);
        }

        // ---------- CART ----------

        public IActionResult Cart()
        {
            CartVM vm = new CartVM();
            vm.Items = new List<CartItemVM>();

            CartItemVM item1 = new CartItemVM();
            item1.Id = 1;
            item1.Name = "APPLE iPhone 17 5G 256 GB Mist Blue";
            item1.Variant = "Mist Blue";
            item1.UnitPrice = 939m;
            item1.Quantity = 1;
            item1.LineTotal = item1.UnitPrice * item1.Quantity;
            vm.Items.Add(item1);

            CartItemVM item2 = new CartItemVM();
            item2.Id = 2;
            item2.Name = "Sony WH-1000XM5 Wireless Headphones";
            item2.Variant = "Black";
            item2.UnitPrice = 349m;
            item2.Quantity = 2;
            item2.LineTotal = item2.UnitPrice * item2.Quantity;
            vm.Items.Add(item2);

            decimal subtotal = 0m;
            for (int i = 0; i < vm.Items.Count; i++)
            {
                subtotal += vm.Items[i].LineTotal;
            }
            vm.Subtotal = subtotal;

            vm.Shipping = (subtotal == 0m || subtotal >= 50m) ? 0m : 4.95m;
            vm.Total = vm.Subtotal + vm.Shipping;

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

        private List<Product> ProductsBySlug(params string[] categorySlugs)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variations)
                .Where(p => p.Category != null && categorySlugs.Contains(p.Category.Slug))
                .ToList();
        }

        private CategoryPageVM BuildCategoryPage(string name, string action, string[] filters, List<Product> products)
        {
            CategoryPageVM vm = new CategoryPageVM();
            vm.CategoryName = name;
            vm.Action = action;
            vm.Products = ToProductCards(products);
            vm.Filters = filters.ToList();
            return vm;
        }

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
                result.Add(card);
            }
            return result;
        }

        private List<CategoryTileVM> BuildCategoryTiles()
        {
            List<CategoryTileVM> tiles = new List<CategoryTileVM>();

            CategoryTileVM t1 = new CategoryTileVM();
            t1.Name = "Computer"; t1.Action = "CompLap"; t1.Span = 1;
            tiles.Add(t1);

            CategoryTileVM t2 = new CategoryTileVM();
            t2.Name = "Laptop"; t2.Action = "CompLap"; t2.Span = 1;
            tiles.Add(t2);

            CategoryTileVM t3 = new CategoryTileVM();
            t3.Name = "Smartphone"; t3.Action = "PhoneWear"; t3.Span = 1;
            tiles.Add(t3);

            CategoryTileVM t4 = new CategoryTileVM();
            t4.Name = "Wearables"; t4.Action = "PhoneWear"; t4.Span = 1;
            tiles.Add(t4);

            CategoryTileVM t5 = new CategoryTileVM();
            t5.Name = "TV"; t5.Action = "TVaudio"; t5.Span = 1;
            tiles.Add(t5);

            CategoryTileVM t6 = new CategoryTileVM();
            t6.Name = "Audio"; t6.Action = "TVaudio"; t6.Span = 2;
            tiles.Add(t6);

            CategoryTileVM t7 = new CategoryTileVM();
            t7.Name = "Household Appliances"; t7.Action = "HA"; t7.Span = 1;
            tiles.Add(t7);

            CategoryTileVM t8 = new CategoryTileVM();
            t8.Name = "Gaming"; t8.Action = "GameDivert"; t8.Span = 1;
            tiles.Add(t8);

            CategoryTileVM t9 = new CategoryTileVM();
            t9.Name = "Entertainment"; t9.Action = "GameDivert"; t9.Span = 1;
            tiles.Add(t9);

            return tiles;
        }
    }
}
