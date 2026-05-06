using System.Collections.Generic;
using System.Diagnostics;
using MediaBizzare.Models;
using MediaBizzare.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediaBizzare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ---------- HOME ----------

        public IActionResult Index()
        {
            HomeIndexVM vm = new HomeIndexVM();
            vm.Categories = BuildCategoryTiles();

            List<ProductCardVM> all = MockCatalog();
            List<ProductCardVM> best = new List<ProductCardVM>();
            for (int i = 0; i < all.Count && i < 8; i++)
            {
                best.Add(all[i]);
            }
            vm.Bestsellers = best;

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
            CategoryPageVM vm = BuildCategoryPage(
                "Computer & laptop",
                "CompLap",
                new string[] { "Laptops", "Desktops", "Monitors", "Accessories" });
            return View("Category", vm);
        }

        public IActionResult PhoneWear()
        {
            CategoryPageVM vm = BuildCategoryPage(
                "Smartphone & wearables",
                "PhoneWear",
                new string[] { "Smartphones", "Smartwatches", "Earbuds", "Accessories" });
            return View("Category", vm);
        }

        public IActionResult TVaudio()
        {
            CategoryPageVM vm = BuildCategoryPage(
                "TV & audio",
                "TVaudio",
                new string[] { "Televisions", "Speakers", "Soundbars", "Headphones" });
            return View("Category", vm);
        }

        public IActionResult HA()
        {
            CategoryPageVM vm = BuildCategoryPage(
                "Household appliances",
                "HA",
                new string[] { "Kitchen", "Laundry", "Cleaning", "Small appliances" });
            return View("Category", vm);
        }

        public IActionResult GameDivert()
        {
            CategoryPageVM vm = BuildCategoryPage(
                "Game & entertainment",
                "GameDivert",
                new string[] { "Consoles", "Games", "Controllers", "VR" });
            return View("Category", vm);
        }

        // ---------- PRODUCT DETAIL ----------

        public IActionResult Product(int id = 1)
        {
            List<ProductCardVM> all = MockCatalog();
            ProductCardVM card = all[0];
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Id == id)
                {
                    card = all[i];
                    break;
                }
            }

            ProductDetailVM vm = new ProductDetailVM();
            vm.Id = card.Id;
            vm.Name = card.Name;
            vm.CategoryName = card.Category;
            vm.CategoryAction = "Categories";
            vm.Price = card.Price;
            vm.OriginalPrice = card.OriginalPrice;
            vm.Rating = 3.5;
            vm.ReviewCount = 10;
            vm.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse varius enim in eros elementum tristique. Duis cursus, mi quis viverra ornare, eros dolor interdum nulla, ut commodo diam libero vitae erat.";
            vm.Images = new List<string> { card.ImageUrl, "", "", "", "" };
            vm.Variants = new List<string> { "Black", "Blue", "Silver", "White" };

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

            if (subtotal == 0m || subtotal >= 50m)
            {
                vm.Shipping = 0m;
            }
            else
            {
                vm.Shipping = 4.95m;
            }
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

        // ---------- HELPERS (mock data) ----------

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

        private CategoryPageVM BuildCategoryPage(string name, string action, string[] filters)
        {
            List<ProductCardVM> products = MockCatalog();
            for (int i = 0; i < products.Count; i++)
            {
                products[i].Category = name;
            }

            List<string> filterList = new List<string>();
            for (int i = 0; i < filters.Length; i++)
            {
                filterList.Add(filters[i]);
            }

            CategoryPageVM vm = new CategoryPageVM();
            vm.CategoryName = name;
            vm.Action = action;
            vm.Products = products;
            vm.Filters = filterList;
            return vm;
        }

        private List<ProductCardVM> MockCatalog()
        {
            List<ProductCardVM> items = new List<ProductCardVM>();
            for (int i = 1; i <= 8; i++)
            {
                ProductCardVM p = new ProductCardVM();
                p.Id = i;
                p.Slug = "apple-iphone-17-" + i;
                p.Name = "APPLE iPhone 17 - 5G - 256 GB Mist Blue";
                p.Price = 939m;
                p.OriginalPrice = 969m;
                p.Category = "Smartphone & wearables";

                if (p.OriginalPrice.HasValue)
                {
                    p.Saving = p.OriginalPrice.Value - p.Price;
                }
                else
                {
                    p.Saving = 0m;
                }
                p.OnSale = p.Saving > 0m;

                items.Add(p);
            }
            return items;
        }
    }
}
