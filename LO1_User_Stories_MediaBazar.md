# User Stories: Media Bazar

**LO mapped:** LO1 Analysing (User Specifications, Documenting Specifications)

## Overview

This doc lists the frontend-facing user stories for Media Bazar, the online electronics marketplace we're building as a group project. It's written from my angle as the frontend developer, so the stories focus on what a shopper can see and do on the site. Each story gets a short acceptance-criteria list and a MoSCoW priority so we know what's blocking the MVP and what can wait.

The stories are grouped by feature area: Listings, Search and Categories, Cart, and Admin (the admin stuff is my groupmate's area but I've included the shell of it for completeness). Auth stories aren't in here yet because the login and signup pages haven't been built on my side, they're waiting on the backend contract.

## MoSCoW priorities

- **Must have.** The app can't ship the MVP without it.
- **Should have.** Expected by the MVP but not strictly blocking.
- **Could have.** Nice to include if time allows.
- **Won't have (this semester).** Explicitly parked.

## Listings

### US-001: Browse the homepage

**As a** shopper
**I want** to land on a homepage with a clear hero, category tiles, and a bestsellers strip
**So that** I get a feel for the store in the first few seconds without hunting for a menu

**Priority:** Must have
**Acceptance criteria:**
- The homepage loads at `/` or `/Home/Index` without errors.
- Hero banner, category tile grid, and bestsellers carousel are visible.
- Clicking a category tile routes to the matching category listing page.

### US-002: View a category listing

**As a** shopper
**I want** to see all products in a category on one page with a filter sidebar
**So that** I can narrow down to what I actually want

**Priority:** Must have
**Acceptance criteria:**
- Routes like `/Home/CompLap`, `/Home/PhoneWear`, `/Home/TVaudio` load a shared category view.
- The product grid renders cards for every product in scope.
- The filter sidebar renders (the filters themselves don't have to work in MVP, but they have to render).

### US-003: Open a product detail page

**As a** shopper
**I want** to click a product card and see a detail view with gallery, variants, price, and an add-to-cart button
**So that** I can decide if it's what I want before buying

**Priority:** Must have
**Acceptance criteria:**
- `/Home/Product/{id}` loads with the product data for that ID.
- The page shows the product name, price, variant chips, quantity stepper, and add-to-cart button.
- Clicking a variant chip visibly sets it as active.

### US-004: See a "save X%" badge when something's discounted

**As a** shopper
**I want** the product card to show a savings badge when there's a discount
**So that** I can spot deals without doing math

**Priority:** Should have
**Acceptance criteria:**
- If `OnSale` is true on the view model, a `.SavePill` renders on the card.
- If not on sale, no badge is shown.
- The percentage is set by the controller, not computed in the view.

## Search and Categories

### US-005: Use a header search input

**As a** shopper
**I want** a search input in the header that I can type in
**So that** the site feels like a real store even before search is wired up

**Priority:** Should have
**Acceptance criteria:**
- The header renders a search input on every page.
- The input is styled to match the Figma design.
- Submitting the form doesn't break anything (even if search isn't implemented yet).

[NOTE: Search is currently decorative. Once backend exposes a search endpoint, this story gets tightened to include real results.]

### US-006: View an all-categories landing page

**As a** shopper
**I want** a single page that shows every category tile
**So that** I have one place to start browsing if I don't know what I'm looking for

**Priority:** Should have
**Acceptance criteria:**
- `/Home/Categories` renders with a grid of all category tiles.
- Each tile links to the matching category page.
- The layout matches the Figma mockup.

## Cart

### US-007: Add items to cart from any page

**As a** shopper
**I want** to press an "add to cart" button on a product card or detail page and see the cart count go up
**So that** I have feedback that the action worked

**Priority:** Must have
**Acceptance criteria:**
- Any element with `data-add-to-cart` triggers the cart increment handler.
- The cart badge in the header updates immediately.
- The badge stays hidden when the count is zero.

### US-008: View the cart page

**As a** shopper
**I want** to open a cart page that shows my line items, quantities, and order totals
**So that** I know exactly what I'm about to pay for

**Priority:** Must have
**Acceptance criteria:**
- `/Home/Cart` renders a list of cart line items with name, qty, and line total.
- An order summary section shows subtotal, shipping, and total.
- The totals are calculated in the controller, not in the view.

### US-009: Change item quantity on the cart page

**As a** shopper
**I want** to increase or decrease item quantity with plus and minus buttons on the cart page
**So that** I can fix mistakes without having to re-add items

**Priority:** Should have
**Acceptance criteria:**
- Every line item has a qty stepper with a minimum of 1 and a maximum of 99.
- Clicking the plus or minus button changes the visible quantity.
- The quantity field is capped at 99 so nobody accidentally buys 50,000 TVs.

[NOTE: The line-total recalc on quantity change isn't wired up yet. That needs a server round trip or a JS math pass. Flag as partial in the acceptance doc when we revisit.]

### US-010: Remove an item from the cart

**As a** shopper
**I want** a remove button on each cart row
**So that** I can take things out without emptying the whole cart

**Priority:** Must have
**Acceptance criteria:**
- Every cart row renders a `.Cart-remove` button.
- Clicking removes the row from the DOM.
- The cart badge decreases by one.

## Admin

### US-011: Admin sign-in (shell only)

**As an** admin
**I want** a login page that takes my credentials
**So that** I can get into the admin area to manage products

**Priority:** Won't have (this semester, on my side)
**Acceptance criteria:**
- n/a until backend auth is wired up.

[NOTE: Admin pages are my groupmate's area. Keeping the story here so the portfolio reflects the full product, not just my slice.]

## Validation

These stories were pulled out of the Figma designs and the group's initial feature brief. I walked through them with my groupmates at the end of the setup sprint to confirm we agreed on what counts as "done" for each one. The coach hasn't formally acceptance-tested them yet, that's scheduled for the review meeting.

[NOTE: Drop the real date of the last walkthrough you did with your groupmates or the coach. If you haven't done one yet, schedule it and then note it here.]
