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
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // Returns the category that belongs to the department managed by the current user.
        private async Task<Category?> GetManagedCategoryAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
            if (employee == null) return null;
            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.ManagerId == employee.Id);
            if (dept == null) return null;
            return await _context.Categories.FirstOrDefaultAsync(c => c.DepartmentId == dept.Id);
        }

        // GET: Products
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                if (cat == null)
                    return View(new List<Product>());
                query = query.Where(p => p.CategoryId == cat.Id);
            }

            return View(await query.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                if (cat == null || product.CategoryId != cat.Id)
                    return Forbid();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                if (cat == null) return Forbid();
                ViewData["CategoryId"] = new SelectList(new[] { cat }, "Id", "Name");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            }
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Slug,Description")] Product product)
        {
            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                if (cat == null || product.CategoryId != cat.Id)
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                ViewData["CategoryId"] = cat != null
                    ? new SelectList(new[] { cat }, "Id", "Name", product.CategoryId)
                    : new SelectList(Array.Empty<Category>(), "Id", "Name");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                if (cat == null || product.CategoryId != cat.Id)
                    return Forbid();
                ViewData["CategoryId"] = new SelectList(new[] { cat }, "Id", "Name", product.CategoryId);
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Slug,Description")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                // Always re-fetch CategoryId from DB — DM cannot move a product to another category.
                var dbProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (dbProduct == null) return NotFound();

                var cat = await GetManagedCategoryAsync();
                if (cat == null || dbProduct.CategoryId != cat.Id)
                    return Forbid();

                product.CategoryId = dbProduct.CategoryId;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                var cat = await GetManagedCategoryAsync();
                ViewData["CategoryId"] = cat != null
                    ? new SelectList(new[] { cat }, "Id", "Name", product.CategoryId)
                    : new SelectList(Array.Empty<Category>(), "Id", "Name");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
                _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
