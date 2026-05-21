using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace Neveria.Services
{
    public class SaleService : ISaleService
    {
        private readonly DbFreezeDreamContext _context;
        public SaleService(DbFreezeDreamContext context) => _context = context;

        public async Task<List<VentaResumenDTO>> GetAllAsync() =>
            await _context.Sales
                .Include(s => s.TagEmployeeNavigation)
                    .ThenInclude(e => e.TagUserNavigation)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.TagProductNavigation)
                .OrderByDescending(s => s.DateSale)
                .Select(s => new VentaResumenDTO
                {
                    TaglSale       = s.TaglSale,
                    DateSale       = s.DateSale,
                    DueTotal       = s.DueTotal,
                    NombreEmpleado = s.TagEmployeeNavigation.TagUserNavigation.Name,
                    Detalles       = s.SaleDetails.Select(sd => new VentaDetalleDTO
                    {
                        NameProduct = sd.TagProductNavigation.NameProduct,
                        Quantity    = sd.Quantity,
                        Price       = sd.Price
                    }).ToList()
                })
                .ToListAsync();

        public async Task<VentaResumenDTO?> GetByIdAsync(int id) =>
            await _context.Sales
                .Where(s => s.TaglSale == id)
                .Include(s => s.TagEmployeeNavigation)
                    .ThenInclude(e => e.TagUserNavigation)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.TagProductNavigation)
                .Select(s => new VentaResumenDTO
                {
                    TaglSale       = s.TaglSale,
                    DateSale       = s.DateSale,
                    DueTotal       = s.DueTotal,
                    NombreEmpleado = s.TagEmployeeNavigation.TagUserNavigation.Name,
                    Detalles       = s.SaleDetails.Select(sd => new VentaDetalleDTO
                    {
                        NameProduct = sd.TagProductNavigation.NameProduct,
                        Quantity    = sd.Quantity,
                        Price       = sd.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

        public async Task<List<VentasPorProductoDTO>> GetVentasPorProductoAsync() =>
            await _context.SaleDetails
                .Include(sd => sd.TagProductNavigation)
                    .ThenInclude(p => p.TagCategorieNavigation)
                .GroupBy(sd => new
                {
                    sd.TagProductNavigation.NameProduct,
                    sd.TagProductNavigation.TagCategorieNavigation.NameCategorie
                })
                .Select(g => new VentasPorProductoDTO
                {
                    NameProduct   = g.Key.NameProduct,
                    NameCategorie = g.Key.NameCategorie,
                    TotalUnidades = g.Sum(sd => sd.Quantity),
                    TotalIngresos = g.Sum(sd => sd.Quantity * sd.Price)
                })
                .OrderByDescending(x => x.TotalUnidades)
                .ToListAsync();

        // Datos para el select de empleados en la vista Create
        public async Task<List<object>> GetEmpleadosSelectAsync()
        {
            var empleados = await _context.Employees
                .Include(e => e.TagUserNavigation)
                .Select(e => new
                {
                    value = e.TagEmployee,
                    text  = e.TagUserNavigation.Name
                })
                .ToListAsync();
            return empleados.Cast<object>().ToList();
        }

        // Datos de productos activos con stock para el carrito JSON
        public async Task<List<object>> GetProductosActivosAsync()
        {
            var productos = await _context.Products
                .Include(p => p.Inventory)
                .Where(p => p.IsActive)
                .Select(p => new
                {
                    tagProduct  = p.TagProduct,
                    nameProduct = p.NameProduct,
                    unitPrice   = p.UnitPrice,
                    stock       = p.Inventory != null ? p.Inventory.StockQuantity : 0
                })
                .ToListAsync();
            return productos.Cast<object>().ToList();
        }

        // Crea una venta completa con sus detalles y descuenta el inventario
        public async Task<bool> CreateAsync(IFormCollection form)
        {
            try
            {
                int tagEmployee = int.Parse(form["TagEmployee"]);
                decimal dueTotal = 0;

                var sale = new Sale
                {
                    TagEmployee = tagEmployee,
                    DateSale    = DateTime.Now,
                    DueTotal    = 0
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync(); // guarda para obtener el TaglSale

                int i = 0;
                while (form.ContainsKey($"SaleDetails[{i}].TagProduct"))
                {
                    int tagProduct = int.Parse(form[$"SaleDetails[{i}].TagProduct"]);
                    int quantity   = int.Parse(form[$"SaleDetails[{i}].Quantity"]);
                    decimal price  = decimal.Parse(form[$"SaleDetails[{i}].Price"]);

                    _context.SaleDetails.Add(new SaleDetail
                    {
                        TaglSale   = sale.TaglSale,
                        TagProduct = tagProduct,
                        Quantity   = quantity,
                        Price      = price
                    });

                    dueTotal += quantity * price;

                    // Descuenta el inventario
                    var inventario = await _context.Inventories
                        .FirstOrDefaultAsync(inv => inv.TagProduct == tagProduct);
                    if (inventario != null)
                    {
                        inventario.StockQuantity -= quantity;
                        inventario.UpdateAt       = DateTime.Now;
                    }

                    i++;
                }

                sale.DueTotal = dueTotal;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR AL GUARDAR VENTA: " + ex.Message);
                return false;
            }
        }

        // Elimina la venta y sus detalles (respeta FK)
        public async Task<bool> DeleteAsync(int id)
        {
            var detalles = await _context.SaleDetails
                .Where(d => d.TaglSale == id)
                .ToListAsync();
            _context.SaleDetails.RemoveRange(detalles);

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null) return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }
        // Stats rápidas para el dashboard
        public async Task<GraficosEstadisticasDTO> GetEstadisticasAsync()
        {
            var ahora = DateTime.Now;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
            var inicioMesAnterior = inicioMes.AddMonths(-1);

            // Ventas este mes (cantidad de registros en Sales)
            var ventasMes = await _context.Sales
                .Where(s => s.DateSale >= inicioMes)
                .CountAsync();

            // Ingresos totales
            var ingresos = await _context.Sales.SumAsync(s => s.DueTotal);

            // Producto más vendido
            var masVendido = await _context.SaleDetails
                .Include(sd => sd.TagProductNavigation)
                .GroupBy(sd => sd.TagProductNavigation.NameProduct)
                .Select(g => new { Nombre = g.Key, Total = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Total)
                .FirstOrDefaultAsync();

            // Ventas por semana este mes y mes anterior
            var ventasPorSemanaEsteMes = await GetVentasPorSemanaAsync(inicioMes, ahora);
            var ventasPorSemanaMesAnt = await GetVentasPorSemanaAsync(inicioMesAnterior, inicioMes);

            return new GraficosEstadisticasDTO
            {
                VentasMes = ventasMes,
                IngresosTotales = ingresos,
                ProductoMasVendido = masVendido?.Nombre ?? "—",
                VentasSemanaActual = ventasPorSemanaEsteMes,
                VentasSemanaAnterior = ventasPorSemanaMesAnt
            };
        }

        private async Task<List<int>> GetVentasPorSemanaAsync(DateTime inicio, DateTime fin)
        {
            var ventas = await _context.Sales
                .Where(s => s.DateSale >= inicio && s.DateSale < fin)
                .ToListAsync();

            var semanas = new List<int> { 0, 0, 0, 0 };
            foreach (var v in ventas)
            {
                int dia = (v.DateSale - inicio).Days;
                int semana = Math.Min(dia / 7, 3); // semanas 0-3
                semanas[semana]++;
            }
            return semanas;
        }

        // Ventas recientes para la tabla
        public async Task<List<VentaDetalleTablaDTO>> GetVentasRecientesAsync(int top = 10) =>
            await _context.SaleDetails
                .Include(sd => sd.TagProductNavigation)
                .Include(sd => sd.TaglSaleNavigation)
                .OrderByDescending(sd => sd.TaglSaleNavigation.DateSale)
                .Take(top)
                .Select(sd => new VentaDetalleTablaDTO
                {
                    NameProduct = sd.TagProductNavigation.NameProduct,
                    Quantity = sd.Quantity,
                    Price = sd.Price,
                    Total = sd.Quantity * sd.Price,
                    DateSale = sd.TaglSaleNavigation.DateSale
                })
                .ToListAsync();
    }
}
