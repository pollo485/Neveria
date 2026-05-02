using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService  = productService;
            _categoryService = categoryService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // Select de categorías para los modales
            var cats = await _categoryService.GetAllAsync();
            ViewData["TagCategorie"] = new SelectList(cats, "CategoryId", "CategoryName");

            var products = await _productService.GetAllRawAsync();
            return View(products);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TagProduct,TagCategorie,NameProduct,UnitPrice,DescriptionProduct,IsActive")] Product product)
        {
            ModelState.Remove("TagCategorieNavigation");

            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            var cats = await _categoryService.GetAllAsync();
            ViewData["TagCategorie"] = new SelectList(cats, "CategoryId", "CategoryName", product.TagCategorie);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("TagProduct,TagCategorie,NameProduct,UnitPrice,DescriptionProduct,IsActive")] Product product)
        {
            if (id != product.TagProduct) return NotFound();
            ModelState.Remove("TagCategorieNavigation");

            if (ModelState.IsValid)
            {
                var result = await _productService.EditAsync(id, product);
                if (!result) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            var cats = await _categoryService.GetAllAsync();
            ViewData["TagCategorie"] = new SelectList(cats, "CategoryId", "CategoryName", product.TagCategorie);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
