# Views and Partials Hierarchy: Media Bazar

**LO mapped:** LO2 Designing (Software Designs, Maintainability)

## Why this exists

Media Bazar is ASP.NET Core MVC, no React, so "component hierarchy" here means Razor views and partials instead of JSX components. The idea is the same though. If I know which partials repeat across pages and which bits are page-specific, I can change a shared piece in one place without hunting through six `.cshtml` files. Planning this before writing the views saved me from copy-pasting the product card layout into four different pages.

## Shared pieces

These live in `Views/Shared/` and get pulled into pages with `<partial name="..." />`.

- `_Layout.cshtml`. The outer shell. Header, subnav, footer, and the body slot. Every page renders inside this.
- `_ProductCard.cshtml`. Product card. Used on the homepage bestsellers, on the category listing grid, and on the cart-related recommendation area (not built yet).
- `_CategoryTile.cshtml`. Category tile. Used on the homepage category grid and the all-categories landing page.
- `_ValidationScriptsPartial.cshtml`. Microsoft template default, left alone. I haven't needed client validation yet on my side.

Inside the layout itself, the header and footer are defined as one chunk of Razor per section. If either grows, they should get split into their own partials (`_Header.cshtml`, `_Footer.cshtml`). Right now they're small enough that splitting would just add indirection.

[NOTE: If you split the header or footer into its own partial later, update this list.]

## Homepage (`Views/Home/Index.cshtml`)

**Purpose:** Landing page. Hero banner, category tiles, bestsellers strip.

**Tree:**

```
Index.cshtml
├── (Layout) Header + SubNav
├── .Hero
│   ├── .Hero-title
│   └── .Hero-cta (Btn)
├── .Section (category grid)
│   └── .CatGrid
│       └── partial: _CategoryTile.cshtml (repeats)
├── .Section (bestsellers)
│   └── .Bestsellers
│       ├── .Bestsellers-head
│       ├── .Bestsellers-carousel (id: bestCarousel)
│       │   └── partial: _ProductCard.cshtml (repeats)
│       └── .Bestsellers-arrows (bestPrev, bestNext)
└── (Layout) Footer
```

**Why this shape:**

The hero is inline because it only appears here. The category tiles and bestseller cards both use partials because they show up on other pages too. The carousel arrows are siblings of the scroll container so the JS can find both with plain `getElementById` calls.

## Category pages (`Views/Home/Category.cshtml`)

**Purpose:** One view that serves all five category routes (CompLap, PhoneWear, TVaudio, HA, GameDivert). The controller picks the data, the view renders it.

**Tree:**

```
Category.cshtml
├── (Layout) Header + SubNav
├── .Breadcrumb
├── .CategoryPage
│   ├── .CategoryPage-sidebar (filters)
│   │   └── .CategoryPage-filterGroup (repeats)
│   └── .CategoryPage-main
│       ├── .CategoryPage-toolbar
│       └── .ProductGrid
│           └── partial: _ProductCard.cshtml (repeats)
└── (Layout) Footer
```

**Why this shape:**

I used one view for five routes instead of five near-identical files. The controller hands in the page title and the filter list, the view just renders what it gets. Less duplication, one place to change when the filter layout needs a tweak.

The five passthrough files (`CompLap.cshtml`, `PhoneWear.cshtml`, etc.) only exist as a safety net in case `HomeController.CompLap` ever gets reverted to `return View();` instead of `return View("Category", ...)`. They each render the Category partial with the passed model.

## Product detail (`Views/Home/Product.cshtml`)

**Purpose:** Single product view. Gallery, variant chips, quantity stepper, add-to-cart.

**Tree:**

```
Product.cshtml
├── (Layout) Header + SubNav
├── .Breadcrumb
├── .ProductView
│   ├── .ProductView-gallery
│   │   ├── .ProductView-mainImage
│   │   └── .ProductView-thumbnails
│   ├── .ProductView-details
│   │   ├── .ProductView-name
│   │   ├── .ProductView-price
│   │   ├── .ProductView-variants (chip row)
│   │   │   └── .Chip (repeats)
│   │   ├── .Qty (stepper)
│   │   │   ├── .QtyDec
│   │   │   ├── input
│   │   │   └── .QtyInc
│   │   └── .Btn (data-add-to-cart)
│   └── .ProductView-description
└── (Layout) Footer
```

**Why this shape:**

Gallery on the left, details on the right is the standard e-commerce detail layout, so there's no reason to reinvent it. The variant chips and the qty stepper are grouped into their own blocks because their JS handlers target those exact containers.

## Cart (`Views/Home/Cart.cshtml`)

**Purpose:** Cart page with line items and an order summary.

**Tree:**

```
Cart.cshtml
├── (Layout) Header + SubNav
├── .Breadcrumb
├── .Cart
│   ├── .Cart-items
│   │   └── .Cart-row (repeats)
│   │       ├── .Cart-rowImage
│   │       ├── .Cart-rowInfo
│   │       ├── .Cart-qty
│   │       ├── .Cart-lineTotal
│   │       └── .Cart-remove
│   └── .Cart-summary
│       ├── .Cart-summary-line (subtotal)
│       ├── .Cart-summary-line (shipping)
│       ├── .Cart-summary-total
│       └── .Btn (checkout)
└── (Layout) Footer
```

**Why this shape:**

Items on the left, summary on the right, stacked on mobile. The summary is its own block because it's going to get reused on a checkout page later, once there is one.

## Categories landing (`Views/Home/Categories.cshtml`)

**Purpose:** All-categories landing page. Reuses the same tiles the homepage uses but in a fuller grid.

**Tree:**

```
Categories.cshtml
├── (Layout) Header + SubNav
├── .Breadcrumb
├── .Section
│   └── .CatGrid
│       └── partial: _CategoryTile.cshtml (repeats)
└── (Layout) Footer
```

No page-specific blocks. Just the shared tile partial laid out in a grid.

## Privacy (`Views/Home/Privacy.cshtml`)

Placeholder doc page. Uses a `.Doc` block for readable text-heavy layout. Not interesting structurally.

## Cross-page patterns

Three patterns show up across the views and are worth calling out:

`_ProductCard.cshtml` is the heaviest-reused partial. It lives on the homepage bestsellers, on every category grid, and will live on any "related products" block that gets added later. That's why I made sure the view model (`ProductCardVM`) was totally plain with no computed properties. If the card needs to render off different data shapes later (e.g. a bestseller row vs a search result row), the controller just maps whatever source into `ProductCardVM` and the partial doesn't care.

The `.Breadcrumb` block appears on category, product, cart, and categories landing. Every time the user navigates deeper than the homepage, they get a way back. I haven't extracted it into a partial because the content is one line of page-specific text and extracting would just be indirection. If the breadcrumb gets smarter (e.g. auto-generated from the route), extracting makes sense.

The layout header/footer pattern means adding a new page is basically "create `Views/Home/<Name>.cshtml`, reference a view model, write the body". The cross-page chrome is handled for me.

## What I'd change

The five category passthrough files (CompLap.cshtml etc.) are redundant scaffolding. They only exist because I wasn't sure the first time whether the routing would work with `return View("Category", vm)`. Now that it does, they're effectively dead code. If I come back to this, I'd delete them and trust the controller.

The Cart summary block (subtotal, shipping, total) should probably become its own partial before the checkout page ever gets built. Right now it's inline in `Cart.cshtml`. Pulling it out is a 10-minute refactor but I haven't done it because checkout isn't a priority yet.

[NOTE: If you refactor either of these before submission, delete or update this section.]
