using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaBizzare.Data;
using MediaBizzare.Models;
using MediaBizzare.Models.ViewModels;

namespace MediaBizzare.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public MyAccountController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /MyAccount/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var vm = new EditProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Street = user.Street,
                StreetNumber = user.StreetNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                Country = user.Country,
            };
            return View(vm);
        }

        // POST: /MyAccount/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.Name = vm.Name;
            user.Surname = vm.Surname;
            user.PhoneNumber = vm.PhoneNumber;
            user.Street = vm.Street;
            user.StreetNumber = vm.StreetNumber;
            user.PostalCode = vm.PostalCode;
            user.City = vm.City;
            user.Country = vm.Country;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(vm);
            }

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Edit));
        }

        // GET: /MyAccount/EditEmployee
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EditEmployee()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                TempData["Info"] = "No employee record found for your account. Please contact HR.";
                return RedirectToAction(nameof(Edit));
            }

            var vm = new EditEmployeeViewModel
            {
                EmployeeId = employee.Id,
                JobTitle = employee.JobTitle,
                EmergencyContact = employee.EmergencyContact,
                EmergencyPhone = employee.EmergencyPhone,
            };
            return View(vm);
        }

        // POST: /MyAccount/EditEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Always look up by the current user's identity — never trust the submitted EmployeeId.
            var userId = int.Parse(_userManager.GetUserId(User)!);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null) return NotFound();

            employee.JobTitle = vm.JobTitle;
            employee.EmergencyContact = vm.EmergencyContact;
            employee.EmergencyPhone = vm.EmergencyPhone;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Employee details updated successfully.";
            return RedirectToAction(nameof(EditEmployee));
        }
    }
}
