using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DbFreezeDreamContext _context;

        public ProductsController(DbFreezeDreamContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // Cargamos la lista para el select de los modales
            ViewData["TagCategorie"] = new SelectList(_context.Categories, "TagCategorie", "NameCategorie");

            // IMPORTANTE: .Include para traer el nombre de la categoría
            var products = await _context.Products.Include(p => p.TagCategorieNavigation).ToListAsync();
            return View(products);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagProduct,TagCategorie,NameProduct,UnitPrice,DescriptionProduct,IsActive")] Product product)
        {
            // Quitamos la navegación para que no falle la validación (igual que en Users)
            ModelState.Remove("TagCategorieNavigation");

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TagCategorie"] = new SelectList(_context.Categories, "TagCategorie", "NameCategorie", product.TagCategorie);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagProduct,TagCategorie,NameProduct,UnitPrice,DescriptionProduct,IsActive")] Product product)
        {
            if (id != product.TagProduct) return NotFound();

            ModelState.Remove("TagCategorieNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.TagProduct)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagCategorie"] = new SelectList(_context.Categories, "TagCategorie", "NameCategorie", product.TagCategorie);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.TagProduct == id);
        }
    }
}