using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaBizzare.Data;
using MediaBizzare.Models;

namespace MediaBizzare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductVariationsController : Controller
    {
        private readonly MediaBizzareContext _context;

        public ProductVariationsController(MediaBizzareContext context)
        {
            _context = context;
        }

        // GET: ProductVariations
        public async Task<IActionResult> Index()
        {
            var mediaBizzareContext = _context.ProductVariations.Include(p => p.Product);
            return View(await mediaBizzareContext.ToListAsync());
        }

        // GET: ProductVariations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariations == null)
            {
                return NotFound();
            }

            return View(productVariations);
        }

        // GET: ProductVariations/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description");
            return View();
        }

        // POST: ProductVariations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,SKU,Price,Stock")] ProductVariations productVariations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productVariations.ProductId);
            return View(productVariations);
        }

        // GET: ProductVariations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productVariations.ProductId);
            return View(productVariations);
        }

        // POST: ProductVariations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,SKU,Price,Stock")] ProductVariations productVariations)
        {
            if (id != productVariations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariationsExists(productVariations.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productVariations.ProductId);
            return View(productVariations);
        }

        // GET: ProductVariations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariations == null)
            {
                return NotFound();
            }

            return View(productVariations);
        }

        // POST: ProductVariations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations != null)
            {
                _context.ProductVariations.Remove(productVariations);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariationsExists(int id)
        {
            return _context.ProductVariations.Any(e => e.Id == id);
        }
    }
}
