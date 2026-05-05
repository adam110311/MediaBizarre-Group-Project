# Frontend Implementation Showcase: Media Bazar

**LO mapped:** LO3 Implementation (Iterative Software Development Process)

## Overview

This is a walkthrough of the pages I've built on my side of Media Bazar so far. The stack is ASP.NET Core MVC 8 with Razor views, hand-written CSS split across three files, and one plain-JS file for the small behaviours (mobile menu, cart badge, variant chips, quantity steppers, carousel). Bootstrap 5 and jQuery are still in `wwwroot/lib` from the default template but I barely use them. Most of the styling is custom because the Figma design wants shapes that Bootstrap's utility classes don't match.

The build is at the "core shopping flow works on mocked data" stage. The controller's `MockCatalog()` and `BuildCategoryTiles()` helpers feed the views so the frontend runs standalone. When my groupmate's backend is ready, those helpers get swapped for real repository calls and the view models stay the same.

## Homepage

**Status:** Working (on mocked data)
**Route:** `/` or `/Home/Index`

![Homepage screenshot](screenshots/mediabazar-home.png)

[NOTE: Drop a screenshot of the homepage running locally here.]

The homepage renders a hero banner with the primary CTA, a category tile grid that pulls from `BuildCategoryTiles()`, and a bestsellers carousel that loops the mocked product list. The carousel is a horizontal-scroll container with two arrow buttons that shift `scrollLeft` by 520px when pressed. Nothing fancy, no slider library, no keyboard handlers yet.

Both the category tiles and the bestseller cards render through partials (`_CategoryTile.cshtml` and `_ProductCard.cshtml`), so any visual change to a card updates every page at once.

## Categories landing

**Status:** Working
**Route:** `/Home/Categories`

![Categories landing screenshot](screenshots/mediabazar-categories.png)

[NOTE: Screenshot of `/Home/Categories`.]

One simple page. Breadcrumb, section header, full grid of category tiles. The tiles are the same `_CategoryTile.cshtml` partial the homepage uses, so there's no duplicated markup.

## Category listing

**Status:** Working (filters render, filtering not wired)
**Route:** `/Home/CompLap`, `/Home/PhoneWear`, `/Home/TVaudio`, `/Home/HA`, `/Home/GameDivert`

![Category listing screenshot](screenshots/mediabazar-category.png)

[NOTE: Screenshot of one of the category pages, probably CompLap since it has the most product types.]

All five category routes share one view file (`Category.cshtml`). The controller action for each route calls a shared `BuildCategoryPage(...)` helper, passing in the page title and the filter list for that category. The view gets a `CategoryPageVM` with everything it needs.

The filter sidebar renders but doesn't actually filter the product grid yet. I flagged this as a "Should have" in the user stories doc. Once the backend has a query endpoint that takes filter params, the filters get wired to a form post or a small JS call.

## Product detail

**Status:** Working
**Route:** `/Home/Product/{id}`

![Product detail screenshot](screenshots/mediabazar-product.png)

[NOTE: Screenshot of a product detail page. Pick one that shows the variant chips and quantity stepper.]

Product detail has the gallery on the left, details on the right. The variant chips use a simple single-select JS pattern where clicking a chip removes `Chip-active` from its siblings and adds it to the clicked one. The quantity stepper clamps between 1 and 99. The add-to-cart button fires the same handler as the product cards.

The page gets its data from the controller's `MockCatalog()` helper via `GetById()`, which is a plain for-loop lookup. No LINQ. If the id doesn't match anything, the action returns `NotFound()`.

## Cart

**Status:** Working (add, remove, quantity; no checkout)
**Route:** `/Home/Cart`

![Cart screenshot](screenshots/mediabazar-cart.png)

[NOTE: Screenshot of the cart page with at least two items in it.]

The cart page lists line items on the left and an order summary on the right. Each row has a quantity stepper, a line total, and a remove button. The summary shows subtotal, shipping, and total. All totals are computed in the controller before the view renders, not in the view itself. That was a deliberate call to keep the view dumb and the math testable.

Line-total recalc on quantity change isn't live yet. Right now the stepper changes the visible number but the line total doesn't update until you reload. That's the next thing to wire once there's a real cart endpoint.

## Privacy

**Status:** Placeholder
**Route:** `/Home/Privacy`

Default Microsoft template page with the text swapped out. Not interesting, included here for completeness.

## Evolution: before and after

The frontend went through one real rewrite worth documenting. My first pass used BEM-style class names (`__`, `--`), CSS variables for the color tokens, computed getters on the view models, and some LINQ in the controller for cart totals. All of that is fine in a vacuum, but none of it matched the level of MVC I'm actually being taught in class, and my groupmate hadn't seen any of those patterns either. If either of us needed to pair on a file, the syntax would become its own hurdle.

**Before (BEM + CSS variables):**

```css
.product-card {
  background: var(--color-bg-soft);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
}

.product-card__media {
  aspect-ratio: 4 / 3;
}

.product-card--featured {
  box-shadow: var(--shadow-md);
}
```

**After (PascalCase-hyphen + hardcoded hex):**

```css
.ProductCard {
  background: #FFFFFF;
  border: 1px solid #E6E6E6;
  border-radius: 12px;
}

.ProductCard-media {
  aspect-ratio: 4 / 3;
}

.ProductCard-featured {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}
```

The naming convention matches what I'm already using on Grubs4Scrubs, my individual project. Keeping one naming style across both codebases made my head hurt less.

**Before (view model with computed property):**

```csharp
public class CartVM
{
    public List<CartItemVM> Items { get; set; } = new();
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
}
```

**After (plain POCO, controller does the math):**

```csharp
public class CartVM
{
    public List<CartItemVM> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
}
```

Same reasoning. The view reads, the controller writes. If a teammate opens the file, there's no expression-bodied getter to squint at.

## What's next

Things that aren't built yet on my side and what's blocking them:

- **Login and signup pages.** Blocked on the backend auth endpoints. I don't want to build screens that'll have to change shape once the real contract is in place.
- **Search.** The input renders and the form posts, but no results page exists. Needs a search endpoint.
- **Filter functionality.** Filters render but don't filter. Needs a query endpoint that takes the filter params.
- **Checkout.** The cart's checkout button goes nowhere. Needs a checkout flow spec from my groupmate plus a backend order endpoint.
- **Real product images.** Every image is a placeholder SVG right now. Swapping them is a 20-minute job once the product table has image paths.

These are frontend plumbing tasks that all need a backend contract first. Once the contract's stable, the work is mostly wiring up forms and handlers to real endpoints.
