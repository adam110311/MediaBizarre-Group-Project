# Technical Decisions: Media Bazar

**LO mapped:** LO3 Implementation (Relevant Software Principles), LO2 Designing

## Scope

This doc covers the bigger "why" decisions I made on the frontend side of Media Bazar. It's not every single line of code, it's the choices where I had a real option and picked one for a reason I can defend. Each decision is tied back to a principle (Single Responsibility, Separation of Concerns, KISS, DRY) because that's what LO3 is actually asking for.

I'm only covering my side of the project (frontend, Razor views, CSS, JS, view models). The backend decisions are my groupmate's call.

## Decision: View models are plain POCOs, not expression-bodied

**Context:** The cart view needs a subtotal, a shipping cost, and a total. All three can be derived from the line items.

**Options considered:**
- Use computed properties on the view model (`public decimal Subtotal => Items.Sum(i => i.LineTotal);`).
- Use plain properties and let the controller set them before passing the model to the view.

**Choice:** Plain properties, controller does the math.
**Reasoning:** Two reasons. First, the class currently covers computed getters but I haven't used them in a project yet, and my groupmate hasn't either. If either of us has to open the cart view mid-sprint, a plain `public decimal Subtotal { get; set; }` is faster to reason about. Second, keeping the view dumb and the controller in charge of calculation means the math lives in one testable spot. If I ever want to unit test the cart total logic, I just test the controller method.
**SOLID / OO principle tied to this:** Separation of Concerns. The view renders, the controller computes. Nothing on the view model does work.

## Decision: For loops instead of LINQ in the controller

**Context:** The controller needs to look up products by ID, filter category lists, and iterate cart items to compute totals.

**Options considered:**
- Use LINQ (`.FirstOrDefault`, `.Where`, `.Select`, `.Sum`).
- Use explicit `for` loops and `if` checks.

**Choice:** Explicit for loops.
**Reasoning:** LINQ is fine and in a lot of codebases it's the clearer choice. The reason I went the other way here is that the MVC module I'm in right now hasn't covered LINQ yet, and the rest of my classroom code is all loops and conditionals. Keeping the style consistent with what's taught means the controller reads like the rest of my coursework, which matters when a teacher reviews it or my groupmate has to patch a bug.
**SOLID / OO principle tied to this:** KISS. Simpler code for the reader, at the cost of a few more lines.

## Decision: Hardcoded hex colors instead of CSS custom properties

**Context:** The design has a fixed palette. Navy `#163251`, orange `#F58220`, light blue `#9AC8F3`, a few greys. I could use CSS variables or just write the hex values directly.

**Options considered:**
- Define CSS custom properties (`--color-primary: #163251`) in `:root` and reference them everywhere.
- Write hex values directly in every rule.

**Choice:** Hex values directly.
**Reasoning:** I use hex values directly in Grubs4Scrubs too, and I wanted one pattern across both codebases. CSS variables are objectively better for a production system with a design team, but for a student project with a known fixed palette, find-and-replace on a hex code does the same job with less indirection. If the project grows and the palette expands, switching to variables is a one-hour refactor.
**SOLID / OO principle tied to this:** YAGNI. I'm not abstracting something that doesn't need abstraction yet.

## Decision: One shared category view instead of five separate files

**Context:** Media Bazar has five product categories (CompLap, PhoneWear, TVaudio, HA, GameDivert). Each needs a listing page with a title, a filter list, and a product grid.

**Options considered:**
- Write five near-identical `.cshtml` files, one per category.
- Write one `Category.cshtml` view and have five controller actions that pass different `CategoryPageVM` data to the same view.

**Choice:** One shared view.
**Reasoning:** The five pages differ only by title, filter list, and product subset. Duplicating the layout five times means every future style change to the category page has to be made in five places. One view with five controller actions means the shape stays consistent and any visual tweak hits every category at once.
**SOLID / OO principle tied to this:** DRY. The view is the abstraction, the controller provides the data.

## Decision: Partials for product cards and category tiles

**Context:** Product cards appear on the homepage bestsellers strip, on every category listing page, and will appear on recommendation blocks later. Category tiles appear on the homepage grid and on the categories landing page.

**Options considered:**
- Inline the card/tile markup in every view that uses it.
- Extract to Razor partials (`_ProductCard.cshtml`, `_CategoryTile.cshtml`) and pull them in with `<partial name="..." />`.

**Choice:** Partials.
**Reasoning:** Even two uses is enough to justify extracting. Three or four uses and not extracting would be negligent. The partial takes a view model, renders markup, and nothing else. The parent view hands in a `ProductCardVM` or `CategoryTileVM` and doesn't care how the card renders internally. If the card's structure changes, I change the partial once.
**SOLID / OO principle tied to this:** Single Responsibility. The partial renders one thing. It doesn't know about the page it's on.

## Decision: Plain JS in one file, no module system, no framework

**Context:** The frontend has five small interactive behaviours: mobile menu toggle, cart badge, variant chip single-select, quantity steppers, and carousel arrows.

**Options considered:**
- Add a small JS framework (Alpine, htmx, Stimulus) for the interactions.
- Write vanilla JS with ES6 modules.
- Write vanilla JS as a single flat file with `var` declarations and `.onclick` handlers.

**Choice:** Single flat file, `var`, `.onclick`.
**Reasoning:** The project scope is small. Five interactions, nothing complex, no state shared across pages beyond a volatile cart count. Adding a framework would mean learning its conventions, configuring a build, and pulling it into a project that doesn't otherwise need one. ES6 modules would mean either a build step or a `type="module"` script tag, both of which are more than the interactions deserve. The current `site.js` is readable top to bottom in 30 seconds.
**SOLID / OO principle tied to this:** YAGNI. I'm not adding tooling for a problem I don't have.

## Decision: Cart state in memory, no persistence on the frontend

**Context:** The cart badge needs to show a count. Items added to the cart need to be remembered across page transitions within a session. At some point the cart needs to survive a page reload.

**Options considered:**
- Persist cart state in `localStorage` on the client.
- Persist on the server and refetch on every page load.
- Keep an in-memory counter for now, wire up server-side persistence once the backend has a cart endpoint.

**Choice:** In-memory counter, placeholder until backend is ready.
**Reasoning:** `localStorage` would give me persistence across reloads right now, but it would also mean the cart logic lives in two places (JS for client persistence, server for eventual checkout). Once the backend has a cart endpoint, the JS side becomes a dumb caller and the server owns the state. Building server-side from the start is the right call, even though it means the cart currently resets on every reload. I'd rather have a partial feature that's correct than a complete feature that's going to be refactored in two weeks.
**SOLID / OO principle tied to this:** Separation of Concerns. State of record lives on the server, the client just reflects it.

## Decisions I'd revisit

A couple of these don't feel great even now.

The five category passthrough files (`CompLap.cshtml`, `PhoneWear.cshtml`, etc.) shouldn't exist. I added them as a belt-and-braces fallback in case the routing ever reverted to `return View();` instead of `return View("Category", vm)`. It doesn't, so they're dead code. Next time I'd trust the routing and delete them.

The choice to skip CSS variables looks cleaner to me now than it did when I first wrote the hex everywhere. If the design ever gets a dark mode or a seasonal theme, I'm going to regret it. Probably worth converting before the project grows more. [NOTE: If you do the conversion, update this doc.]

The add-to-cart handler is currently attached with `.onclick` per-element in a for loop, which means if the page ever renders new cards dynamically (e.g. infinite scroll on a category page), those cards won't have handlers attached. A single delegated listener on a parent container would fix it. Not a problem yet because nothing renders cards dynamically, but I'd do it that way from the start next time.
