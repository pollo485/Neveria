using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

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
                .Select(s => new VentaResumenDTO
                {
                    TaglSale = s.TaglSale,
                    DateSale = s.DateSale,
                    DueTotal = s.DueTotal,
                    NombreEmpleado = s.TagEmployeeNavigation.TagUserNavigation.Name,
                    Detalles = s.SaleDetails.Select(sd => new VentaDetalleDTO
                    {
                        NameProduct = sd.TagProductNavigation.NameProduct,
                        Quantity = sd.Quantity,
                        Price = sd.Price
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
                    TaglSale = s.TaglSale,
                    DateSale = s.DateSale,
                    DueTotal = s.DueTotal,
                    NombreEmpleado = s.TagEmployeeNavigation.TagUserNavigation.Name,
                    Detalles = s.SaleDetails.Select(sd => new VentaDetalleDTO
                    {
                        NameProduct = sd.TagProductNavigation.NameProduct,
                        Quantity = sd.Quantity,
                        Price = sd.Price
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
                    NameProduct = g.Key.NameProduct,
                    NameCategorie = g.Key.NameCategorie,
                    TotalUnidades = g.Sum(sd => sd.Quantity),
                    TotalIngresos = g.Sum(sd => sd.Quantity * sd.Price)
                })
                .OrderByDescending(x => x.TotalUnidades)
                .ToListAsync();
    }
}