# Version Control Evidence: Media Bazar

**LO mapped:** LO4 Managing (Version Control)

## Repository

**URL:** [NOTE: Add the actual GitHub URL for Media Bazar here.]
**Visibility:** [NOTE: Private or Public? Fill in.]
**Hosted on:** GitHub

The repo was set up at the start of the semester by one of my groupmates. It has a `.gitignore` tuned for ASP.NET Core (so the `bin/`, `obj/`, and `.vs/` folders stay out of commits) and a minimal `README.md` with setup instructions for the project. Everyone on the team cloned it on day one, and we've all been pushing to it since.

[NOTE: If the README has been updated or a docs folder exists, mention it here.]

## Branching strategy

Because this is a group project and we've had a history of stepping on each other's changes, we settled on a lightweight feature-branch model on top of `main`. Nobody pushes directly to `main`. Features go on their own branch, get merged back when they're ready, and hotfixes get their own branch too.

```
main
├── feature/frontend-layout
├── feature/product-views
├── feature/cart-page
├── feature/backend-employees
├── feature/backend-auth
└── hotfix/routing-bug
```

- **`main`.** The integrated state. Protected, or at least treated as protected. Everyone agreed no direct pushes early on.
- **`feature/<name>`.** New work. Branched off main, merged back when the feature is done.
- **`hotfix/<name>`.** Quick fixes that can't wait for a feature branch.

The split between frontend and backend branches is mostly practical. My groupmate and I were working on genuinely separate parts of the code (I touched views, CSS, JS, and view models; they touched models, the DB context, and the controllers that don't render views). Keeping those on different branches meant we rarely had merge conflicts, and the ones we did have were small.

[NOTE: If your groupmate used a different branching pattern and you want to describe it honestly, adjust this section. Don't claim strict Git-flow if that's not what happened.]

## Commit practices

My commits follow the "one logical change per commit" rule most of the time. I try to write messages in the present-tense imperative ("Add product detail view") rather than past tense, and I skip the body unless the change needs context.

**Commit messages I've actually made on this project:**

- `Add shared _ProductCard and _CategoryTile partials`
- `Rewrite home controller without LINQ, use for loops for mock catalog lookup`
- `Rename CSS classes to PascalCase-hyphen convention, update all cshtml references`
- `Add cart page with inline totals calculated in controller`
- `Fix mobile menu toggle on small screens (class name was out of sync)`

[NOTE: Open the GitHub repo and grab 5 to 8 real commit messages from your own commits. Swap these examples with the real ones. Anything I wrote above is a reasonable-sounding guess, not the actual history.]

Not every commit I make is clean. Sometimes I push a WIP commit at the end of the day so I don't lose work, and then squash or amend on the next push. I try to clean those up before merging into `main` but it doesn't always happen.

## Commit history

![Commit history screenshot](screenshots/commit-history.png)

[NOTE: Screenshot of a chunk of the repo's commit history. Easiest place to grab this is github.com/<repo>/commits/main. Pick a stretch that shows a mix of feature commits and shows both me and my groupmate contributing.]

The history shows regular activity across the whole semester, with bursts of commits around the sprint boundaries. You can see the frontend and backend work happening mostly on separate branches before merging, which is the pattern we agreed on.

## Pull requests and merges

Most merges happen through GitHub's PR interface. When a feature branch is ready, I open a PR into `main`, wait for my groupmate to take a quick look (or I do the same for theirs), and then merge. For small changes we sometimes just merge without a formal review, which is fine for a team of this size but I'd tighten that up in a larger project.

![PR screenshot](screenshots/pr-example.png)

[NOTE: Screenshot of one of the pull requests in the repo. Either your own or your groupmate's. Open the PR view and snap the title, description, and the green "merged" status.]

We've had one or two merge conflicts over the semester, both on the `Views/Home/HomeController.cs` file because both of us were editing it from different angles. We resolved them by pulling `main` into the feature branch locally, reviewing the conflict together, and picking the right version line by line.

## What I'd improve

A few things aren't great and I'd do differently next time.

My commit messages sometimes get lazy on the last day of a sprint. "Fix stuff" or "small tweaks" show up in the log once or twice and that's not useful to anyone. If I'd stuck to the discipline I started with, every commit message would explain what changed without needing to read the diff.

Feature branches sometimes live longer than they should. A branch that's been open for a week drifts further from `main` the longer it sits, and when it finally gets merged the conflicts are bigger than they need to be. The fix is to merge smaller, merge more often, and not treat a feature branch as a long-running workspace.

We don't do code review formally. For a school project it's probably fine, but it's a habit I want to build before I end up on a real team. Even a 30-second look at a PR catches typos and naming inconsistencies that would otherwise make it to main.

[NOTE: If any of these got better toward the end of the semester, update the section. Honesty is the point here.]
