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
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
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

        // GET: Departments
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Department> query = _context.Departments
                .Include(d => d.Manager)
                .ThenInclude(m => m!.User);

            if (!User.IsInRole("Admin"))
            {
                var dept = await GetManagedDepartmentAsync();
                if (dept == null)
                    return View(new List<Department>());
                query = query.Where(d => d.Id == dept.Id);
            }

            return View(await query.ToListAsync());
        }

        // GET: Departments/Details/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Manager)
                .ThenInclude(m => m!.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create()
        {
            ViewData["ManagerId"] = await BuildManagerSelectListAsync();
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Create([Bind("Id,ManagerId,Name,Slug,Description")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = await BuildManagerSelectListAsync(department.ManagerId);
            return View(department);
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            // DepartmentManager can only edit the department they manage.
            if (!User.IsInRole("Admin"))
            {
                var managed = await GetManagedDepartmentAsync();
                if (managed == null || managed.Id != department.Id)
                    return Forbid();
            }

            ViewData["ManagerId"] = await BuildManagerSelectListAsync(department.ManagerId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DepartmentManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ManagerId,Name,Slug,Description")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            // DepartmentManager can only edit the department they manage.
            if (!User.IsInRole("Admin"))
            {
                var managed = await GetManagedDepartmentAsync();
                if (managed == null || managed.Id != id)
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["ManagerId"] = await BuildManagerSelectListAsync(department.ManagerId);
            return View(department);
        }

        // GET: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Manager)
                .ThenInclude(m => m!.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        // Builds a SelectList of employees showing "First Last — Job Title" as display text.
        private async Task<List<SelectListItem>> BuildManagerSelectListAsync(int? selectedId = null)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.User!.Name + " " + e.User.Surname + " — " + e.JobTitle,
                    Selected = selectedId.HasValue && e.Id == selectedId.Value
                })
                .ToListAsync();
        }
    }
}
