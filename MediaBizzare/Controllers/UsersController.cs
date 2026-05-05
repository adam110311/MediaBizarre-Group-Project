using System.Threading.Tasks;
using MediaBizzare.Data;
using MediaBizzare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Controllers
{
    // Admin-only user management. Password operations are NOT exposed here on purpose —
    // those go through Identity (UserManager). This controller covers viewing and editing
    // profile/domain fields only.
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly MediaBizzareContext _context;

        public UsersController(MediaBizzareContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Bind only the editable profile fields. Password/security fields are NOT bindable here.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,UserName,Email,PhoneNumber,Name,Surname,BankAccount,Street,StreetNumber,PostalCode,City,Country")]
            ApplicationUser user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Load the tracked entity and copy only allowed fields onto it.
                    var existing = await _context.Users.FindAsync(id);
                    if (existing == null) return NotFound();

                    existing.UserName = user.UserName;
                    existing.Email = user.Email;
                    existing.PhoneNumber = user.PhoneNumber;
                    existing.Name = user.Name;
                    existing.Surname = user.Surname;
                    existing.BankAccount = user.BankAccount;
                    existing.Street = user.Street;
                    existing.StreetNumber = user.StreetNumber;
                    existing.PostalCode = user.PostalCode;
                    existing.City = user.City;
                    existing.Country = user.Country;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
