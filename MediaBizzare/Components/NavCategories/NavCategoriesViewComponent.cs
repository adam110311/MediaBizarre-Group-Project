using MediaBizzare.Data;
using MediaBizzare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Components.NavCategories
{
    /// <summary>
    /// Renders the sub-nav category links from the database so the nav
    /// always stays in sync with the Categories table.
    /// </summary>
    public class NavCategoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public NavCategoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList();

            // Pass the active slug so the view can highlight the current category.
            var currentSlug = HttpContext.Request.Query["slug"].ToString();
            ViewData["CurrentSlug"] = currentSlug;

            return View(categories);
        }
    }
}
