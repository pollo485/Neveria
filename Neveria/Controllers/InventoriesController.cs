using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService   _productService;

        public InventoriesController(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService   = productService;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            // Select de productos que aún NO tienen inventario registrado
            var idsConInventario = await _inventoryService.GetProductIdsWithInventoryAsync();
            var todosProductos   = await _productService.GetAllRawAsync();
            var sinInventario    = todosProductos
                .Where(p => !idsConInventario.Contains(p.TagProduct))
                .Select(p => new { p.TagProduct, p.NameProduct });

            ViewData["TagProduct"] = new SelectList(sinInventario, "TagProduct", "NameProduct");

            var inventario = await _inventoryService.GetAllAsync();
            return View(inventario);
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var inventory = await _inventoryService.GetRawByIdAsync(id.Value);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        // GET: Inventories/Create
        public async Task<IActionResult> Create()
        {
            var idsConInventario = await _inventoryService.GetProductIdsWithInventoryAsync();
            var todosProductos   = await _productService.GetAllRawAsync();
            var sinInventario    = todosProductos
                .Where(p => !idsConInventario.Contains(p.TagProduct))
                .Select(p => new { p.TagProduct, p.NameProduct });

            ViewData["TagProduct"] = new SelectList(sinInventario, "TagProduct", "NameProduct");
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
                await _inventoryService.CreateAsync(inventory);

            return RedirectToAction(nameof(Index));
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var inventory = await _inventoryService.GetRawByIdAsync(id.Value);
            if (inventory == null) return NotFound();

            var todosProductos = await _productService.GetAllRawAsync();
            ViewData["TagProduct"] = new SelectList(todosProductos, "TagProduct", "NameProduct", inventory.TagProduct);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("TagInventory,TagProduct,StockQuantity,MinQuantity,UpdateAt")] Inventory inventory)
        {
            if (id != inventory.TagInventory) return NotFound();
            ModelState.Remove("TagProductNavigation");

            if (ModelState.IsValid)
            {
                var result = await _inventoryService.EditAsync(id, inventory);
                if (!result) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            var todosProductos = await _productService.GetAllRawAsync();
            ViewData["TagProduct"] = new SelectList(todosProductos, "TagProduct", "NameProduct", inventory.TagProduct);
            return RedirectToAction(nameof(Index));
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var inventory = await _inventoryService.GetRawByIdAsync(id.Value);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
