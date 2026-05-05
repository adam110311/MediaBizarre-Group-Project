<<<<<<< HEAD
// Media Bazar - small frontend behaviours.
// Mobile menu, cart count badge, variant chips, quantity steppers, carousel.

var cartCount = 0;

function updateCartBadge() {
    var badge = document.getElementById("cartBadge");
    if (badge == null) {
        return;
    }
    badge.textContent = cartCount;
    if (cartCount > 0) {
        badge.style.display = "inline-flex";
    } else {
        badge.style.display = "none";
    }
}

// Mobile menu toggle

var menuToggle = document.getElementById("menuToggle");
var navLinks = document.getElementById("navLinks");
if (menuToggle != null && navLinks != null) {
    menuToggle.onclick = function () {
        if (navLinks.classList.contains("SubNav-links-open")) {
            navLinks.classList.remove("SubNav-links-open");
        } else {
            navLinks.classList.add("SubNav-links-open");
        }
    };
}

// Add-to-cart buttons (anywhere on the page)

var addButtons = document.querySelectorAll("[data-add-to-cart]");
for (var i = 0; i < addButtons.length; i++) {
    addButtons[i].onclick = function (e) {
        e.preventDefault();
        cartCount = cartCount + 1;
        updateCartBadge();
    };
}

// Remove cart row buttons

var removeButtons = document.querySelectorAll(".Cart-remove");
for (var j = 0; j < removeButtons.length; j++) {
    removeButtons[j].onclick = function () {
        var row = this.parentElement;
        if (row != null) {
            row.parentElement.removeChild(row);
        }
        if (cartCount > 0) {
            cartCount = cartCount - 1;
        }
        updateCartBadge();
    };
}

// Variant chips - single select inside their row

var variantRows = document.querySelectorAll(".ProductView-variants");
for (var v = 0; v < variantRows.length; v++) {
    var chips = variantRows[v].querySelectorAll(".Chip");
    for (var c = 0; c < chips.length; c++) {
        chips[c].onclick = function () {
            var siblings = this.parentElement.querySelectorAll(".Chip");
            for (var s = 0; s < siblings.length; s++) {
                siblings[s].classList.remove("Chip-active");
            }
            this.classList.add("Chip-active");
        };
    }
}

// Quantity steppers (decrease)

var decButtons = document.querySelectorAll(".QtyDec");
for (var d = 0; d < decButtons.length; d++) {
    decButtons[d].onclick = function () {
        var input = this.parentElement.querySelector("input");
        if (input == null) {
            return;
        }
        var current = parseInt(input.value);
        if (isNaN(current)) {
            current = 1;
        }
        if (current > 1) {
            input.value = current - 1;
        }
    };
}

// Quantity steppers (increase)

var incButtons = document.querySelectorAll(".QtyInc");
for (var n = 0; n < incButtons.length; n++) {
    incButtons[n].onclick = function () {
        var input = this.parentElement.querySelector("input");
        if (input == null) {
            return;
        }
        var current = parseInt(input.value);
        if (isNaN(current)) {
            current = 1;
        }
        if (current < 99) {
            input.value = current + 1;
        }
    };
}

// Bestsellers carousel arrows

var bestCarousel = document.getElementById("bestCarousel");
var bestPrev = document.getElementById("bestPrev");
var bestNext = document.getElementById("bestNext");

if (bestCarousel != null && bestPrev != null && bestNext != null) {
    bestPrev.onclick = function () {
        bestCarousel.scrollLeft = bestCarousel.scrollLeft - 520;
    };
    bestNext.onclick = function () {
        bestCarousel.scrollLeft = bestCarousel.scrollLeft + 520;
    };
}

updateCartBadge();
=======
﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
>>>>>>> 3ef1d535815291eb7c0caca5d67c1bc3e9e52b62
