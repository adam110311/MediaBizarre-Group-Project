# Meeting Notes: Media Bazar

**LO mapped:** LO5 Professional Standard (Project Organisation, Communication)

## How I take meeting notes

I keep meeting notes short and focused on what actually got decided, not a transcript of everything said. The bits I always write down are the attendees, the decisions that came out of the meeting, and the action items (who owns what and when it's due). The rest is just context.

So far we've had two client presentation reviews. A third is scheduled for May. Each one is a chance to show the client progress from the last sprint, collect what they want next, and walk away with a concrete list of changes for the following sprint. Below is what got said and agreed in each.

---

## Meeting: Sprint 1 Presentation Review

**Date:** 18-03-2026
**Attendees:** Me, [NOTE: groupmate's name], [NOTE: client name(s) and any coaches present].
**Duration:** [NOTE: Roughly how long did this one run? Fill in.]
**Purpose:** Present the Sprint 1 build to the client, walk through the competitive analysis, and collect feedback to shape Sprint 2.

### What the client wants

Going page by page through the build, the client flagged several things they want to see by the next sprint.

- Departments should be listed for selection somewhere in the flow. [NOTE: Confirm exactly where, user signup or product tagging or both.]
- Admin / HR should be able to add users, edit them, create departments, and assign managers to those departments.
- The payment process gets added after the customer purchase management system is complete. If we focus on purchase management first, payment comes after.
- Products and departments should have images tied to them.
- The design is entirely up to us. The client isn't handing over mockups or a brand guide, we own the whole visual side.
- There needs to be an employee overview page.
- For the next presentation the client wants a page that shows what we planned versus what we actually shipped.

### Our own feedback after the meeting

My groupmate and I did a short debrief right after the presentation. Takeaways:

- Our sprint requirements were too ambitious for a team our size. We need to adjust scope to match group capacity.
- The website looks attractive and the client said so directly. Good sign, keep the visual direction going.
- The competitive analysis we did during the analysis phase was appreciated by the client. Worth referencing again in future reviews.
- The first sprint's requirements were partly misaligned with what the client actually wanted. We need to review them and re-plan.
- Our client presentation used too much "job language", developer jargon and technical terms that the client didn't care about. Next time we dumb it down so a non-technical stakeholder can follow.
- We didn't prep properly for this one. For Sprint 2 we expected an actual PowerPoint and a proper walkthrough, not an improvised tour.

### Action items

- [ ] Revisit Sprint 1 requirements and trim anything that doesn't fit the team's capacity, me + [NOTE: groupmate], due before Sprint 2 planning.
- [ ] Add departments, the employee overview page, and the admin-side user management to the Sprint 2 backlog, [NOTE: groupmate on the backend side], due Sprint 2.
- [ ] Add image support on products and departments, me on the frontend, due Sprint 2.
- [ ] Build a "planned vs delivered" slide or short page for the next presentation, me, due Sprint 2 review.
- [ ] Prep a PowerPoint and rehearse the walkthrough before Sprint 2 review, me + [NOTE: groupmate], due the morning of the presentation.

### Follow-up

[NOTE: Did you send any written summary to the group or the client after this? If yes, reference or attach it. If not, leave it honest.]

---

## Meeting: Sprint 2 Presentation Review

**Date:** 09-04-2026
**Attendees:** Me, [NOTE: groupmate's name], Frank dlp, [NOTE: any other client reps or coaches present].
**Duration:** [NOTE: Roughly how long did this one run? Fill in.]
**Purpose:** Present the Sprint 2 build, show planned vs delivered from Sprint 1's feedback, collect the next round of feedback.

### What the client wants (important)

- Add a history page, similar to how Amazon shows past orders, so users can track purchased and ordered items.
- CRUD operations on products should be scoped to manager and admin roles only. Employees don't get create / update / delete rights on products.
- Employees should be able to notify when a product is out of stock. Admin and manager then have the power to restock.

### What the client wants (trivial, but worth capturing)

Frank dlp raised a naming concern. He doesn't want us to use the word "admin" in the product. His reasoning was that he doesn't fully understand most systems, and even though basically every system has an admin under the hood, he'd rather we avoid the term in the UI. The plan we landed on: we still build an admin role functionally (it has to exist for permissions to work cleanly), but we rename it to "personnel manager" or "personnel administrator" in the UI, and in the next presentation we focus mainly on the personnel administrator framing. Functionally it's almost identical to a standard admin, just with different labels.

[NOTE: Confirm with groupmate whether the final label in the UI is "personnel manager" or "personnel administrator". The source feedback mentions both, and we should land on one.]

### Action items

- [ ] Design and build the order history page, me on the frontend, [NOTE: groupmate] on the backend data, due Sprint 3.
- [ ] Gate product CRUD behind manager / admin roles, [NOTE: groupmate], due Sprint 3.
- [ ] Build the out-of-stock notification flow for employees plus the restock action for admin / manager, [NOTE: groupmate on data, me on UI], due Sprint 3.
- [ ] Rename "admin" to personnel manager / administrator throughout the UI and the presentation deck, me, due before Sprint 3 review.

### Follow-up

[NOTE: Same question here. If there was a written follow-up to the client or the group, link it. If not, say so.]

---

## Upcoming: Sprint 3 Presentation Review

**Date:** May 2026, 2pm [NOTE: Drop the exact date in once it's confirmed.]
**Attendees:** Me, [NOTE: groupmate], Frank dlp, [NOTE: any other client reps or coaches].
**Purpose:** Present Sprint 3 work, with a focus on the order history, the personnel manager rename, and the out-of-stock / restock flow. Bring the planned-vs-delivered slide the client asked for back in Sprint 1's review.

I'll update this section with the actual attendance, decisions, and action items once the meeting happens.

---

[NOTE: If any additional meetings come up (extra coach check-ins, internal team syncs, standups worth documenting), add them below in the same format.]
