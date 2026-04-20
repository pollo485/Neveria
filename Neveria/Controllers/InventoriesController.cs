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
    public class InventoriesController : Controller
    {
        private readonly DbFreezeDreamContext _context;

        public InventoriesController(DbFreezeDreamContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            // Solo productos que aún no tienen inventario registrado
            var productosConInventario = _context.Inventories.Select(i => i.TagProduct);
            var productosDisponibles   = _context.Products
                .Where(p => !productosConInventario.Contains(p.TagProduct))
                .Select(p => new { p.TagProduct, p.NameProduct });

            ViewData["TagProduct"] = new SelectList(productosDisponibles, "TagProduct", "NameProduct");

            var inventories = _context.Inventories
                .Include(i => i.TagProductNavigation);

            return View(await inventories.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventories
                .Include(i => i.TagProductNavigation)
                .FirstOrDefaultAsync(m => m.TagInventory == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            var productosConInventario = _context.Inventories.Select(i => i.TagProduct);
            var productosDisponibles   = _context.Products
                .Where(p => !productosConInventario.Contains(p.TagProduct))
                .Select(p => new { p.TagProduct, p.NameProduct });

            ViewData["TagProduct"] = new SelectList(productosDisponibles, "TagProduct", "NameProduct");
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TagInventory,TagProduct,StockQuantity,MinQuantity,UpdateAt")] Inventory inventory)
        {
            ModelState.Remove("TagProductNavigation");

            if (ModelState.IsValid)
            {
                inventory.UpdateAt = DateTime.Now;
                _context.Add(inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
                return NotFound();

            ViewData["TagProduct"] = new SelectList(
                _context.Products, "TagProduct", "NameProduct", inventory.TagProduct);

            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("TagInventory,TagProduct,StockQuantity,MinQuantity,UpdateAt")] Inventory inventory)
        {
            if (id != inventory.TagInventory)
                return NotFound();

            ModelState.Remove("TagProductNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    inventory.UpdateAt = DateTime.Now;
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.TagInventory))
                        return NotFound();
                    else
                        throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventories
                .Include(i => i.TagProductNavigation)
                .FirstOrDefaultAsync(m => m.TagInventory == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.TagInventory == id);
        }
    }
}
