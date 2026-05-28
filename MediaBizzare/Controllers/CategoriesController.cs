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
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // Returns the department managed by the current user (null if not a manager).
        private async Task<Department?> GetManagedDepartmentAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
            if (employee == null) return null;
            return await _context.Departments.FirstOrDefaultAsync(d => d.ManagerId == employee.Id);
        }

        // GET: Categories
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Category> query = _context.Categories.Include(c => c.Department);

            if (!User.IsInRole("Admin"))
            {
                var dept = await GetManagedDepartmentAsync();
                if (dept == null)
                    return View(new List<Category>());
                query = query.Where(c => c.DepartmentId == dept.Id);
            }

            return View(await query.ToListAsync());
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var dept = await GetManagedDepartmentAsync();
                if (dept == null || category.DepartmentId != dept.Id)
                    return Forbid();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,DepartmentId,Name,Slug,ImageUrl")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", category.DepartmentId);
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var dept = await GetManagedDepartmentAsync();
                if (dept == null || category.DepartmentId != dept.Id)
                    return Forbid();
                // DM sees form without department dropdown — ViewBag.DepartmentId left null intentionally
                return View(category);
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", category.DepartmentId);
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartmentId,Name,Slug,ImageUrl")] Category category)
        {
            if (id != category.Id) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                // Always load DepartmentId from DB — DM cannot reassign the category to another dept.
                var dbCat = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (dbCat == null) return NotFound();

                var dept = await GetManagedDepartmentAsync();
                if (dept == null || dbCat.DepartmentId != dept.Id)
                    return Forbid();

                category.DepartmentId = dbCat.DepartmentId;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", category.DepartmentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
                _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
