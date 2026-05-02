using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;
using System.Text.Json;

namespace Neveria.Controllers
{
    public class SalesController : Controller
    {
        private readonly DbFreezeDreamContext _context;
        private readonly ISaleService _saleService;

        public SalesController(DbFreezeDreamContext context, ISaleService _saleService)
        {
            _context = context;
        }

        // GET: Sales
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
            // Cargar empleados para el select
            var empleados = await _context.Employees
                .Include(e => e.TagUserNavigation)
                .Select(e => new
                {
                    value = e.TagEmployee,
                    text = e.TagUserNavigation.Name
                })
                .ToListAsync();

            ViewBag.TagEmployee = empleados.Select(e =>
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = e.value.ToString(),
                    Text = e.text
                });

            // Cargar productos con su stock para el JSON del carrito
            var productos = await _context.Products
                .Include(p => p.Inventory)
                .Where(p => p.IsActive == true)
                .Select(p => new
                {
                    tagProduct = p.TagProduct,
                    nameProduct = p.NameProduct,
                    unitPrice = p.UnitPrice,
                    stock = p.Inventory != null
                                  ? p.Inventory.StockQuantity
                                  : 0
                })
                .ToListAsync();

            ViewBag.ProductosJson = JsonSerializer.Serialize(productos);

            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            try
            {
                // 1. Leer datos básicos de la venta
                int tagEmployee = int.Parse(form["TagEmployee"]);
                decimal dueTotal = 0;

                var sale = new Sale
                {
                    TagEmployee = tagEmployee,
                    DateSale = DateTime.Now,
                    DueTotal = 0
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync(); // guarda para obtener el TaglSale

                // 2. Leer los detalles del carrito
                int i = 0;
                while (form.ContainsKey($"SaleDetails[{i}].TagProduct"))
                {
                    int tagProduct = int.Parse(form[$"SaleDetails[{i}].TagProduct"]);
                    int quantity = int.Parse(form[$"SaleDetails[{i}].Quantity"]);
                    decimal price = decimal.Parse(form[$"SaleDetails[{i}].Price"]);

                    var detalle = new SaleDetail
                    {
                        TaglSale = sale.TaglSale,
                        TagProduct = tagProduct,
                        Quantity = quantity,
                        Price = price
                    };

                    _context.SaleDetails.Add(detalle);
                    dueTotal += quantity * price;

                    // 3. Descontar inventario
                    var inventario = await _context.Inventories
                        .FirstOrDefaultAsync(inv => inv.TagProduct == tagProduct);
                    if (inventario != null)
                    {
                        inventario.StockQuantity -= quantity;
                        inventario.UpdateAt = DateTime.Now;
                    }

                    i++;
                }

                // 4. Actualizar el total real
                sale.DueTotal = dueTotal;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ver el error exacto en consola de Visual Studio
                Console.WriteLine("ERROR AL GUARDAR VENTA: " + ex.Message);
                return RedirectToAction(nameof(Create));
            }
        }

        // POST: Sales/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int TaglSale)
        {
            // Primero eliminar los detalles (FK constraint)
            var detalles = await _context.SaleDetails
                .Where(d => d.TaglSale == TaglSale)
                .ToListAsync();

            _context.SaleDetails.RemoveRange(detalles);

            var sale = await _context.Sales.FindAsync(TaglSale);
            if (sale != null)
                _context.Sales.Remove(sale);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
