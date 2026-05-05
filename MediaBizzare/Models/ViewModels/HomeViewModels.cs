using System.Collections.Generic;

namespace MediaBizzare.Models.ViewModels
{
    // Plain view models used by the views. Kept separate from the EF models
    // so the frontend can render with mock data before the database is wired up.

    public class ProductCardVM
    {
        public int Id { get; set; }
        public string Slug { get; set; } = "";
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string Category { get; set; } = "";
        public decimal Saving { get; set; }
        public bool OnSale { get; set; }
    }

    public class CategoryTileVM
    {
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Action { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int Span { get; set; } = 1;
    }

    public class HomeIndexVM
    {
        public List<CategoryTileVM> Categories { get; set; } = new List<CategoryTileVM>();
        public List<ProductCardVM> Bestsellers { get; set; } = new List<ProductCardVM>();
    }

    public class CategoryPageVM
    {
        public string CategoryName { get; set; } = "";
        public string Action { get; set; } = "";
        public List<ProductCardVM> Products { get; set; } = new List<ProductCardVM>();
        public List<string> Filters { get; set; } = new List<string>();
    }

    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string CategoryAction { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Description { get; set; } = "";
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Variants { get; set; } = new List<string>();
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Variant { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }
}
