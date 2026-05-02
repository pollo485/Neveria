using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly DbFreezeDreamContext _context; // solo para Index

        public CategoriesController(ICategoryService categoryService, DbFreezeDreamContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        // GET: Categories — la vista espera IEnumerable<Category>, no el DTO
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var category = await _categoryService.GetByIdAsync(id.Value);
            if (category == null) return NotFound();
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create() => View();

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TagCategorie,NameCategorie,DescriptionCategorie")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("TagCategorie,NameCategorie,DescriptionCategorie")] Category category)
        {
            if (id != category.TagCategorie) return NotFound();
            if (ModelState.IsValid)
            {
                var result = await _categoryService.EditAsync(id, category);
                if (!result) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int TagCategorie)
        {
            await _categoryService.DeleteAsync(TagCategorie);
            return RedirectToAction(nameof(Index));
        }
    }
}