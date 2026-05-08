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
    public class Employee_ContractController : Controller
    {
        private readonly AppDbContext _context;

        public Employee_ContractController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employee_Contract
        public async Task<IActionResult> Index()
        {
            var mediaBizzareContext = _context.EmployeeContracts.Include(e => e.Employee);
            return View(await mediaBizzareContext.ToListAsync());
        }

        // GET: Employee_Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Contract = await _context.EmployeeContracts
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee_Contract == null)
            {
                return NotFound();
            }

            return View(employee_Contract);
        }

        // GET: Employee_Contract/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "EmergencyContact");
            return View();
        }

        // POST: Employee_Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,signature_date,start_date,end_date,salary,hours_per_week,contract_type")] Employee_Contract employee_Contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee_Contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "EmergencyContact", employee_Contract.EmployeeId);
            return View(employee_Contract);
        }

        // GET: Employee_Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Contract = await _context.EmployeeContracts.FindAsync(id);
            if (employee_Contract == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "EmergencyContact", employee_Contract.EmployeeId);
            return View(employee_Contract);
        }

        // POST: Employee_Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,signature_date,start_date,end_date,salary,hours_per_week,contract_type")] Employee_Contract employee_Contract)
        {
            if (id != employee_Contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee_Contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Employee_ContractExists(employee_Contract.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "EmergencyContact", employee_Contract.EmployeeId);
            return View(employee_Contract);
        }

        // GET: Employee_Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Contract = await _context.EmployeeContracts
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee_Contract == null)
            {
                return NotFound();
            }

            return View(employee_Contract);
        }

        // POST: Employee_Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee_Contract = await _context.EmployeeContracts.FindAsync(id);
            if (employee_Contract != null)
            {
                _context.EmployeeContracts.Remove(employee_Contract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Employee_ContractExists(int id)
        {
            return _context.EmployeeContracts.Any(e => e.Id == id);
        }
    }
}
