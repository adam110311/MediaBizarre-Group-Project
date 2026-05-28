// Media Bazar — frontend behaviours
// Mobile menu · cart (session-backed API) · variant chips · qty steppers · carousel

// ── Cart badge ─────────────────────────────────────────────────────────────

function setCartBadge(count) {
    var badge = document.getElementById("cartBadge");
    if (!badge) return;
    badge.textContent = count;
    badge.style.display = count > 0 ? "inline-flex" : "none";
}

// Fetch the real count from the server on every page load.
fetch("/Cart/Count")
    .then(function (r) { return r.json(); })
    .then(function (data) { setCartBadge(data.count); })
    .catch(function () { /* silently ignore if session not ready */ });

// ── Add-to-cart (product cards + product detail) ───────────────────────────

function addToCart(variationId, quantity) {
    if (!variationId || variationId <= 0) return;

    fetch("/Cart/Add", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ variationId: variationId, quantity: quantity || 1 })
    })
        .then(function (r) { return r.json(); })
        .then(function (data) { setCartBadge(data.count); })
        .catch(function (err) { console.error("Add to cart failed", err); });
}

// Product cards — small cart icon button
document.querySelectorAll("[data-variation-id].ProductCard-cart").forEach(function (btn) {
    btn.addEventListener("click", function (e) {
        e.preventDefault();
        addToCart(parseInt(this.dataset.variationId), 1);
    });
});

// Product detail — "Add to cart" button reads the currently selected variant
var addToCartBtn = document.getElementById("addToCartBtn");
if (addToCartBtn) {
    addToCartBtn.addEventListener("click", function () {
        var varId = parseInt(this.dataset.variationId);
        var qty = 1;
        var qtyInput = document.querySelector(".ProductView-cta-row .Qty input");
        if (qtyInput) qty = parseInt(qtyInput.value) || 1;
        addToCart(varId, qty);
    });
}

// ── Variant chips (product detail) ────────────────────────────────────────

document.querySelectorAll(".ProductView-variants").forEach(function (row) {
    row.querySelectorAll(".Chip").forEach(function (chip) {
        chip.addEventListener("click", function () {
            // Highlight
            row.querySelectorAll(".Chip").forEach(function (c) { c.classList.remove("Chip-active"); });
            this.classList.add("Chip-active");

            // Update the add-to-cart button with the selected variation
            if (addToCartBtn) {
                addToCartBtn.dataset.variationId = this.dataset.variationId;
            }

            // Optionally update the displayed price
            var priceEl = document.querySelector(".ProductView-price");
            if (priceEl && this.dataset.price) {
                priceEl.textContent = "€" + parseFloat(this.dataset.price).toFixed(0);
            }
        });
    });
});

// ── Cart page ──────────────────────────────────────────────────────────────

function recalcCartTotals() {
    var subtotal = 0;
    document.querySelectorAll(".Cart-row").forEach(function (row) {
        var unitPrice = parseFloat(row.dataset.unitPrice) || 0;
        var qtyInput  = row.querySelector(".Cart-qty-input");
        var qty       = qtyInput ? (parseInt(qtyInput.value) || 1) : 1;
        var lineTotal = unitPrice * qty;

        var linePriceEl = row.querySelector(".Cart-line-price");
        if (linePriceEl) linePriceEl.textContent = "€" + lineTotal.toFixed(2);

        subtotal += lineTotal;
    });

    var subtotalEl = document.querySelector(".Cart-subtotal-value");
    var totalEl    = document.querySelector(".Cart-total-value");
    var shippingEl = document.querySelector(".Cart-shipping-value");

    var shipping = (subtotal === 0 || subtotal >= 50) ? 0 : 4.95;

    if (subtotalEl) subtotalEl.textContent = "€" + subtotal.toFixed(2);
    if (shippingEl) shippingEl.textContent = shipping === 0 ? "Free" : "€" + shipping.toFixed(2);
    if (totalEl)    totalEl.textContent    = "€" + (subtotal + shipping).toFixed(2);
}

// Remove buttons
document.querySelectorAll(".Cart-remove").forEach(function (btn) {
    btn.addEventListener("click", function () {
        var row   = this.closest(".Cart-row");
        var varId = row ? parseInt(row.dataset.variationId) : 0;
        if (!varId) return;

        fetch("/Cart/Remove", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ variationId: varId })
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                setCartBadge(data.count);
                if (row) row.remove();
                recalcCartTotals();

                // Show empty state if no rows left
                if (document.querySelectorAll(".Cart-row").length === 0) {
                    location.reload();
                }
            })
            .catch(function (err) { console.error("Remove failed", err); });
    });
});

// Quantity decrease
document.querySelectorAll(".QtyDec").forEach(function (btn) {
    btn.addEventListener("click", function () {
        var input = this.parentElement.querySelector("input");
        if (!input) return;
        var val = parseInt(input.value) || 1;
        if (val > 1) {
            input.value = val - 1;
            syncCartQty(input);
        }
    });
});

// Quantity increase
document.querySelectorAll(".QtyInc").forEach(function (btn) {
    btn.addEventListener("click", function () {
        var input = this.parentElement.querySelector("input");
        if (!input) return;
        var val = parseInt(input.value) || 1;
        if (val < 99) {
            input.value = val + 1;
            syncCartQty(input);
        }
    });
});

// Manual quantity input change
document.querySelectorAll(".Cart-qty-input").forEach(function (input) {
    input.addEventListener("change", function () { syncCartQty(this); });
});

function syncCartQty(input) {
    var row   = input.closest(".Cart-row");
    if (!row) return;
    var varId = parseInt(row.dataset.variationId);
    var qty   = parseInt(input.value) || 1;
    if (qty < 1) { input.value = 1; qty = 1; }

    fetch("/Cart/Update", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ variationId: varId, quantity: qty })
    })
        .then(function (r) { return r.json(); })
        .then(function (data) {
            setCartBadge(data.count);
            recalcCartTotals();
        })
        .catch(function (err) { console.error("Update qty failed", err); });
}

// ── Mobile menu ────────────────────────────────────────────────────────────

var menuToggle = document.getElementById("menuToggle");
var navLinks   = document.getElementById("navLinks");
if (menuToggle && navLinks) {
    menuToggle.addEventListener("click", function () {
        navLinks.classList.toggle("SubNav-links-open");
    });
}

// ── Bestsellers carousel ───────────────────────────────────────────────────

var bestCarousel = document.getElementById("bestCarousel");
var bestPrev     = document.getElementById("bestPrev");
var bestNext     = document.getElementById("bestNext");

if (bestCarousel && bestPrev && bestNext) {
    bestPrev.addEventListener("click", function () { bestCarousel.scrollLeft -= 520; });
    bestNext.addEventListener("click", function () { bestCarousel.scrollLeft += 520; });
}
