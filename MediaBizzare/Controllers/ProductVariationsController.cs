using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaBizzare.Data;
using MediaBizzare.Models;

namespace MediaBizzare.Controllers
{
    [Authorize]
    public class ProductVariationsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductVariationsController(AppDbContext context)
        {
            _context = context;
        }

        // Returns the IDs of products that belong to the department managed by the current user.
        private async Task<List<int>> GetManagedProductIdsAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
            if (employee == null) return new List<int>();
            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.ManagerId == employee.Id);
            if (dept == null) return new List<int>();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.DepartmentId == dept.Id);
            if (category == null) return new List<int>();
            return await _context.Products.Where(p => p.CategoryId == category.Id).Select(p => p.Id).ToListAsync();
        }

        // GET: ProductVariations
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Index()
        {
            IQueryable<ProductVariations> query = _context.ProductVariations.Include(p => p.Product);

            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                query = query.Where(pv => productIds.Contains(pv.ProductId));
            }

            return View(await query.ToListAsync());
        }

        // GET: ProductVariations/Details/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariations == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                if (!productIds.Contains(productVariations.ProductId))
                    return Forbid();
            }

            return View(productVariations);
        }

        // GET: ProductVariations/Create
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                ViewData["ProductId"] = new SelectList(products, "Id", "Name");
            }
            else
            {
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            }
            return View();
        }

        // POST: ProductVariations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create([Bind("Id,ProductId,SKU,Price,Stock,ImageUrl")] ProductVariations productVariations)
        {
            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                if (!productIds.Contains(productVariations.ProductId))
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Add(productVariations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                ViewData["ProductId"] = new SelectList(products, "Id", "Name", productVariations.ProductId);
            }
            else
            {
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productVariations.ProductId);
            }
            return View(productVariations);
        }

        // GET: ProductVariations/Edit/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                if (!productIds.Contains(productVariations.ProductId))
                    return Forbid();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                ViewData["ProductId"] = new SelectList(products, "Id", "Name", productVariations.ProductId);
            }
            else
            {
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productVariations.ProductId);
            }
            return View(productVariations);
        }

        // POST: ProductVariations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,SKU,Price,Stock,ImageUrl")] ProductVariations productVariations)
        {
            if (id != productVariations.Id) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                // Always re-fetch ProductId from DB — DM cannot move a variation to another product.
                var dbPv = await _context.ProductVariations.AsNoTracking().FirstOrDefaultAsync(pv => pv.Id == id);
                if (dbPv == null) return NotFound();

                var productIds = await GetManagedProductIdsAsync();
                if (!productIds.Contains(dbPv.ProductId))
                    return Forbid();

                productVariations.ProductId = dbPv.ProductId;
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
                    if (!ProductVariationsExists(productVariations.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                var productIds = await GetManagedProductIdsAsync();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                ViewData["ProductId"] = new SelectList(products, "Id", "Name", productVariations.ProductId);
            }
            else
            {
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productVariations.ProductId);
            }
            return View(productVariations);
        }

        // GET: ProductVariations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productVariations = await _context.ProductVariations
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariations == null) return NotFound();

            return View(productVariations);
        }

        // POST: ProductVariations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productVariations = await _context.ProductVariations.FindAsync(id);
            if (productVariations != null)
                _context.ProductVariations.Remove(productVariations);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariationsExists(int id)
        {
            return _context.ProductVariations.Any(e => e.Id == id);
        }
    }
}
