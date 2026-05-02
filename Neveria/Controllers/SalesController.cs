using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly DbFreezeDreamContext _context; // solo para Index

        public SalesController(ISaleService saleService, DbFreezeDreamContext context)
        {
            _saleService = saleService;
            _context = context;
        }

        // GET: Sales — la vista espera IEnumerable<Sale>, no el DTO
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Sales
                .Include(s => s.TagEmployeeNavigation)
                    .ThenInclude(e => e.TagUserNavigation)
                .Include(s => s.SaleDetails)
                    .ThenInclude(d => d.TagProductNavigation)
                .OrderByDescending(s => s.DateSale)
                .ToListAsync();
            return View(ventas);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create()
        {
            var empleados = await _saleService.GetEmpleadosSelectAsync();
            ViewBag.TagEmployee = empleados.Select(e =>
            {
                dynamic d = e;
                return new SelectListItem
                {
                    Value = d.value.ToString(),
                    Text = d.text
                };
            });

            var productos = await _saleService.GetProductosActivosAsync();
            ViewBag.ProductosJson = JsonSerializer.Serialize(productos);
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            var result = await _saleService.CreateAsync(form);
            return RedirectToAction(result ? nameof(Index) : nameof(Create));
        }

        // POST: Sales/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int TaglSale)
        {
            await _saleService.DeleteAsync(TaglSale);
            return RedirectToAction(nameof(Index));
        }
    }
}