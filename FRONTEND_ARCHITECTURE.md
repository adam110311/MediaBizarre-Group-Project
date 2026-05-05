# Media Bazar Frontend: Architecture & Decisions

This is a write-up of how the frontend side of Media Bazar is put together, and why it's built the way it is. Backend stuff (database, auth, employee management) isn't covered here because it's not my part of the project.

## Stack

ASP.NET Core MVC 8 with Razor views. Bootstrap 5 and jQuery are already in `wwwroot/lib` from the default template, but I barely use Bootstrap classes. The Figma design wants specific shapes that Bootstrap's utilities don't match, so most of the styling is hand-written CSS.

## Folder layout

Everything frontend-related lives in three places:

- `Views/Home/` holds one Razor view per page (Index, Categories, Category, Product, Cart, Privacy, plus the category passthrough files like CompLap and PhoneWear).
- `Views/Shared/` holds the layout and two partials (`_ProductCard.cshtml`, `_CategoryTile.cshtml`) that get reused across pages.
- `wwwroot/css/` and `wwwroot/js/` hold three CSS files (site, components, app) and one JS file.

View models sit in `Models/ViewModels/HomeViewModels.cs`. The EF models (Product, Category, Employee, etc.) in `Models/` are my groupmate's work and I haven't touched them.

## The MVC flow

Every page goes through `HomeController`. The controller builds a view model, hands it to the view, done. Right now the data is mocked inside `MockCatalog()` and `BuildCategoryTiles()` so the frontend works without the database. When the backend gets wired up, those helpers get replaced with actual repository calls. The view models stay the same either way.

The five category routes (CompLap, PhoneWear, TVaudio, HA, GameDivert) all call `BuildCategoryPage(...)` and return the same `Category.cshtml` view. Saves writing the same page five times. The title and filter list get passed in as arguments.

## CSS naming convention

Every class follows `PascalCaseBlock-element` or `PascalCaseBlock-element-modifier`. Examples:

- `.Header`, `.Header-brand`, `.Header-search`
- `.ProductCard`, `.ProductCard-media`, `.ProductCard-priceNow`
- `.Cart`, `.Cart-summary`, `.Cart-lineTotal`

I'm already using this pattern on my individual project (Grubs4Scrubs), so keeping it consistent across both codebases made sense. No BEM (`__`, `--`), no utility framework. The class name tells you what block it belongs to and that's it.

## View models

Plain POCOs. No computed properties, no expression-bodied getters. If a view needs a subtotal, the controller calculates it and sets the property before passing the model. Views only read values.

This was a deliberate call. An earlier draft had things like:

```csharp
public decimal Subtotal => Items.Sum(i => i.LineTotal);
```

That works, but it's a pattern I haven't covered in class yet, and it reads differently from the rest of my code. The dumber version is a plain `public decimal Subtotal { get; set; }` and the controller does the math in a `for` loop. Easier for a teammate to follow, easier to explain during the review.

## Controller style

`HomeController` avoids LINQ. Where I could write `list.Select(...).ToList()`, I write a `for` loop. Where I'd write `list.FirstOrDefault(x => x.Id == id)`, I loop and break. This isn't because LINQ is bad, it's because the group project should read the same way as the rest of the classroom material. If a teammate opens the file, they shouldn't have to stop and ask what `.Select` does.

Same rule for null checks and cart math. Explicit `if/else` instead of ternaries or null-coalescing chains.

## CSS approach

Three files, each with a job:

- `site.css` covers the reset, buttons, chips, breadcrumbs, and section helpers. Global stuff.
- `components.css` covers header, subnav, footer, product card, and category tile. Things that appear on multiple pages.
- `app.css` covers page-specific styles: hero, category page grid, product detail layout, cart layout.

No CSS variables. Every hex colour is written out. Navy is `#163251`, orange is `#F58220`, light blue is `#9AC8F3`. If a colour changes, find-and-replace handles it. Grubs4Scrubs is written the same way, so I'm used to it.

## JavaScript

One file, `site.js`. No IIFE wrapper, no `'use strict'`, no localStorage, no Web Animations API. Just plain functions, `var` declarations, `.onclick` handlers, and `for` loops over `querySelectorAll`.

Things it handles:

- Mobile menu toggle (shows and hides the nav on small screens)
- Cart badge count (in-memory, resets on reload; will get replaced when the backend has a real cart)
- Variant chip single-select on the product page
- Quantity steppers with a 1 to 99 clamp
- Bestsellers carousel arrows

The cart state is intentionally lightweight. Once the backend has a cart endpoint, the add-to-cart handler gets a real call to it instead of just incrementing a counter.

## Pages

| Route | View | Purpose |
|-------|------|---------|
| `/Home/Index` | `Index.cshtml` | Homepage with hero, category tiles, bestsellers carousel |
| `/Home/Categories` | `Categories.cshtml` | All-categories landing |
| `/Home/CompLap` (and the other four) | `Category.cshtml` | Shared category listing with filters sidebar |
| `/Home/Product/{id}` | `Product.cshtml` | Product detail with gallery, variants, qty, add-to-cart |
| `/Home/Cart` | `Cart.cshtml` | Cart with line items, qty editing, order summary |
| `/Home/Privacy` | `Privacy.cshtml` | Placeholder privacy doc |

## What's not done yet

Auth pages (login and signup) aren't built. Those wait until the backend exposes real endpoints. Product images are all placeholder SVGs, same for the category tile art. The search input in the header is decorative, no handler yet. Filters on the category page render but don't actually filter the list. The cart's "Checkout" button goes nowhere for the same reason.

All of these are frontend plumbing jobs that need a backend contract before I can finish them.

## Why the code looks the way it does

Short version: I dumbed an earlier draft down on purpose. The first pass had CSS variables, computed view-model properties, LINQ chains, an IIFE in the JS, ARIA attributes everywhere, and culture-invariant price formatting. All fine in isolation, but none of it matches the level of MVC I'm actually being taught right now. If a teacher or teammate reads the code, it should look like mine. So I stripped out the stuff that didn't, kept the functionality, and renamed the classes to the convention I already use on my other project.

It's still a scaffold. Real data, real images, and real auth come later.
