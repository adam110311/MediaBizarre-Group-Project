# Project Reflection: Media Bazar

**LO mapped:** LO6 Personal Leadership (Feedback and Reflection)

## What this project was

Media Bazar is a group project in Semester 2 at FHICT. It's an online marketplace for electronics (laptops, phones, TVs, home appliances, that kind of thing), built in ASP.NET Core MVC. My role on the team was frontend developer, so I owned the views, the CSS, the JavaScript, the view models, and the controller actions that rendered pages. My groupmate owned the models, the DB context, and the controllers that deal with data access and admin-side logic.

## What I set out to do

Going in, I wanted to build a shopper-facing frontend that looked like the Figma designs, ran on mocked data until the backend was ready, and was clean enough that swapping the mocks for real data would be a small change. I also wanted to keep the code style consistent with what I'm building on my individual project (Grubs4Scrubs), so I could move between the two codebases without my head switching gears.

Secondary goal: keep my commits clean and the branching disciplined, because I know group projects live or die on that kind of hygiene.

## What I actually did

I built the homepage, the categories landing page, the shared category listing view that serves all five category routes, the product detail page, the cart page, and the privacy placeholder. I extracted the product card and category tile into Razor partials so they could be reused across pages without copy-paste. I wrote the full CSS in three hand-written files (site, components, app) with a custom naming convention that matches what I use on Grubs4Scrubs. I also wrote the small JS file that handles the mobile menu, cart badge, variant chips, quantity steppers, and carousel arrows.

There was one full rewrite partway through. My first pass used BEM class names, CSS variables, computed properties on the view models, and LINQ in the controller. It worked, but none of it matched the classroom-level MVC I'm being taught, and my groupmate hadn't seen those patterns either. I rewrote the whole frontend to use plain POCOs, for loops, hardcoded hex colors, and PascalCase-hyphen class names. The functionality stayed the same, the code became more readable for both of us.

What I didn't do: login and signup pages on the frontend, because I didn't want to build screens that would have to change shape once the real auth contract was in place. Also the search input and the filter sidebar render but don't actually filter anything yet. Those are blocked on backend endpoints.

## The hard parts

The rewrite was the hardest decision I made on this project. It meant throwing away maybe 15 hours of clean-ish code because the style was wrong for my teammate and for the classroom level. That was hard to sit with. The easier path would've been to keep what I had and leave it as "my side is just a bit more advanced." I chose to rewrite because a team project with mismatched styles is a team project with friction every time we touch each other's code.

Dealing with a group where the skill distribution isn't even was the other hard thing. I'm not going to name names or complain, but there were moments when I had to decide whether to help unblock someone or to protect my own sprint time. Usually I helped, sometimes I didn't, and I'm still not sure where the right line is.

[NOTE: This is the most personal section. If you want to be more specific about the group dynamic, write it in your own words. Don't leave it generic if there's a real story.]

The Figma-to-CSS work took longer than I expected. The design has a lot of specific shapes, spacings, and badge placements that Bootstrap's utility classes don't match, so I ended up writing almost everything from scratch. That's fine but it meant the visual polish phase was longer than I'd budgeted for.

## The easier-than-expected parts

Razor partials clicked faster than I thought they would. Coming from React, I half-expected them to feel clumsy, but the `<partial name="..." model="..." />` syntax is essentially the same thing minus JSX. Extracting the product card and the category tile into partials was a 20-minute job each, and from that point every new page got to reuse them.

The routing approach also ended up simpler than I feared. I was ready to fight with `[Route]` attributes on controller actions, but for the five-category problem I just made five controller actions that all call the same shared helper and render the same view. No attribute gymnastics needed.

## What I learned as a developer

I learned that "code style" is actually a team concern, not a personal preference. My first instinct is always to use the cleanest pattern I know, and that's fine when I'm alone. On a team, the right pattern is the one both of us can read at 11pm on a sprint deadline without stopping to google syntax. The rewrite taught me that in a way reading about it never would have.

I also got much better at writing dumb, boring view models. There's a real temptation to put computed getters and derived values on the model so the view can just read them, but pushing that logic into the controller means the view is truly just rendering and the math lives in one place. That's easier to test, easier to debug, and easier to modify.

## What I learned as a person

Sprint hygiene matters more than I gave it credit for. Keeping commits small, branches short-lived, and PRs scoped to one thing isn't just a "best practice," it's what keeps two people from rebuilding each other's work by accident. When we had conflicts on the shared controller, they were resolvable in minutes because the commits were small. If we'd been pushing big week-long chunks to `main`, we'd have been untangling it for hours.

I also learned that admitting "I don't know" earlier is faster. When the auth work came up and neither of us had done cookie auth in MVC before, we spent a week vaguely circling around it instead of just asking the coach. When we did ask, we got pointed at the right docs in five minutes.

## What I'd change if I started over

- Set the code style before writing anything. One conversation on day one about naming conventions, which principles to apply, and how dumb to keep the code would've saved the whole rewrite.
- Push for a backend contract in week 2, not week 6. I built screens against imagined data shapes and some of them had to be reshaped when the real shapes came into focus.
- Write a tiny `_ViewModels` cheat sheet in week 1 showing the shape of each page's data. Would've saved a few mid-sprint "wait, what does this view actually need?" moments.
- Review each other's PRs formally, even briefly. We got away with skipping it on a team of two, but the habit would've caught some stuff.

## What's left on the table

Auth pages, search functionality, filter functionality, checkout flow, and real product images. All of them are blocked on either a backend contract, real image assets, or a feature decision my groupmate owns. If the project continued for another sprint, those are the first things I'd wire up.

[NOTE: If any of these got built between now and submission, update this section.]
