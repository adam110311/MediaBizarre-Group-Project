# Media Bazar - Frontend Plan (Boss)

Your role: frontend dev. This doc maps what the repo actually is right now, what you need to build in code, and what evidence you still need to drop into the Fontys portfolio under each LO category.

## Repo reality check

It's an ASP.NET Core MVC project (C# + Razor views + Bootstrap 5 + jQuery). "MVC" = Microsoft's MVC stack, so frontend for you means Razor views (`.cshtml`), the CSS in `wwwroot/css/`, the JS in `wwwroot/js/`, and the shared layout.

What exists:

- `HomeController` with actions for Index, HomePage, Privacy, Categories, CompLap, GameDivert, HA, PhoneWear, TVaudio
- `_Layout.cshtml` with a working navbar (6 category links) and footer
- `Index.cshtml` has a real hero section, "Our Products", "Bestsellers" block, and a footer. Positioned with absolute + vh, which is going to fight you the moment anyone opens it on a different screen
- Every other category view is a 7-line stub that just says "My Son". Yes, really
- `app.css` is 137 lines, all positioning on the homepage
- `site.js` is basically empty

What's broken or fragile:

- Layout uses `position: absolute` with viewport heights everywhere. That breaks responsive. Priority fix
- No product card component yet, no grid, no reusable partials
- Navbar has a typo ("Household Appliacnes") and no active-state styling
- No mobile menu behaviour beyond the default Bootstrap toggler
- Models exist (Product, Category, Employee, User, etc.) but the views aren't wired to them. You'll need ViewModels or pass the models in once backend has data

## Frontend work plan (what to actually build)

Sprint-style, ordered by what unblocks the rest:

**Sprint 1: foundations**

1. Rewrite `_Layout.cshtml` to use a proper responsive navbar with active link highlighting. Fix the "Appliacnes" typo while you're in there
2. Replace absolute positioning in `app.css` with a normal document flow. Use Bootstrap's grid + flex utilities for the hero, then a container-fluid section for the product grid. Keep the vibe, lose the `top: 170vh` hacks
3. Set up a `_ProductCard.cshtml` partial view that takes a `Product` model. Card shows image, name, price, small CTA. Makes every category page reusable
4. Set up a shared CSS variables block at the top of `site.css` (brand colours, radii, spacing) so the rest of the styles stop hardcoding `#1b6ec2` and `#163251` everywhere

**Sprint 2: real pages**

5. Build out `Categories.cshtml` properly: grid of category tiles (one per top-level category), each linking to its category page
6. Build a real category page template (use one view shared by CompLap / PhoneWear / TVaudio / HA / GameDivert via a single action that takes a category param, OR keep the separate actions but all use the same partial). Shows filters on the left, product grid on the right
7. Build a product detail page (not in the current controller yet, add `Product(int id)` action)
8. Add a basic cart page scaffold (even if backend isn't ready, the UI can mock data)

**Sprint 3: polish + auth-aware UI**

9. Login/register views (hook into Identity or whatever auth the group picks)
10. Employee-facing area if your group has a staff dashboard in scope. The models suggest it's in scope (Employee, EmployeeRole, Employee_Contract, Department)
11. Loading states, empty states, form validation styling
12. Accessibility pass: alt text, focus states, colour contrast on that bright blue

**Sprint 4: frontend quality**

13. Lighthouse run + fixes
14. Cross-browser check
15. Component doc (short markdown listing every partial and what it does)

## Portfolio plan (what to add to Canvas)

You currently have these artifacts from the group's shared work:

| LO category | What's there | Role in it |
| --- | --- | --- |
| Analysing | Media Bazar analysis, software analysis, competitor analysis | Probably group work |
| Designing | ERD, mid-fi wireframes, UI mockup | Mockup sounds like yours |
| Implementation | Environment Setup | Thin |
| Managing | Sprint Planning Board | Shared |
| Professional Standard | Project plan | Shared |
| Personal Leadership | Sprint 1 Review pptx | Shared |

Gaps to fill, skewed toward your role (frontend):

**Analysing.** You want at least one piece that's yours. Options: a short frontend-framework comparison doc (why Razor/MVC vs React SPA for this project), or a UI/UX competitor analysis focused on visual patterns (how Coolblue, BCC, MediaMarkt structure their category pages). The second one is more clearly "frontend" and plays nice with the existing competitor analysis.

**Designing.** This is where you want the most evidence. Current three items are fine but you can add: high-fi wireframes / Figma screens, a component library sheet (button, card, form, nav variants), a style guide (colours, type scale, spacing), and a short design rationale doc explaining the choices. Four more pieces easy.

**Implementation.** Currently just environment setup. You need code evidence. Link the repo, write short "What I built" entries for the layout refactor, the product card partial, the category page template, and the hero rebuild. Each one is a separate evidence description pointing at specific commits or PRs.

**Managing.** Sprint planning is there. You can add: your individual sprint backlog snapshot, a retro note from sprint 1 with concrete frontend takeaways, a burndown or task list showing your commitments vs delivery.

**Professional Standard.** Project plan is group-level. Add something specific to your discipline: a testing plan (browser matrix, what you test manually), or a code-review checklist you use for frontend PRs, or accessibility checklist.

**Personal Leadership.** Sprint 1 review deck is there. You can add: a reflection on what went well / badly in sprint 1 (short .md or .docx), a learning-goals doc for sprint 2, a peer feedback note.

## What I suggest you do first

Right now, today:

1. Fix the `_Layout` typo and make the homepage not break on a laptop (Sprint 1 item 1 and 2). Get a clean commit history while you do it so you can cite the commits later
2. Draft one high-fi wireframe / Figma frame for the category page and the product detail page. Even rough ones
3. Write one ED for the UI Mock Up that's already in the portfolio, because you've already done the work and it's free points

After that, Sprint 1 items 3 and 4 (product card partial + CSS variables) are the biggest unlock for the rest of the term.

## Things I need from you before I can help more

- Is the group using Figma, or something else for design?
- Is there a shared auth / Identity setup decision yet (ASP.NET Identity vs custom)?
- Who else touches the frontend? Any conflict risk on `_Layout` or the CSS?
- Sprint length and sprint 2 start date

Once I know that, I can start rewriting `_Layout.cshtml` and `app.css` properly, stub out the product card partial, and produce whichever EDs you want to tackle first.
