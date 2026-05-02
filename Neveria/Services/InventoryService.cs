using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly DbFreezeDreamContext _context;
        public InventoryService(DbFreezeDreamContext context) => _context = context;

        public async Task<List<InventarioDTO>> GetAllAsync() =>
            await _context.Inventories
                .Include(i => i.TagProductNavigation)
                    .ThenInclude(p => p.TagCategorieNavigation)
                .Select(i => new InventarioDTO
                {
                    TagInventory = i.TagInventory,
                    NameProduct = i.TagProductNavigation.NameProduct,
                    NameCategorie = i.TagProductNavigation.TagCategorieNavigation.NameCategorie,
                    StockQuantity = i.StockQuantity,
                    MinQuantity = i.MinQuantity,
                    UpdateAt = i.UpdateAt
                })
                .ToListAsync();

        public async Task<List<InventarioDTO>> GetStockBajoAsync() =>
            await _context.Inventories
                .Where(i => i.StockQuantity <= i.MinQuantity)
                .Include(i => i.TagProductNavigation)
                    .ThenInclude(p => p.TagCategorieNavigation)
                .Select(i => new InventarioDTO
                {
                    TagInventory = i.TagInventory,
                    NameProduct = i.TagProductNavigation.NameProduct,
                    NameCategorie = i.TagProductNavigation.TagCategorieNavigation.NameCategorie,
                    StockQuantity = i.StockQuantity,
                    MinQuantity = i.MinQuantity,
                    UpdateAt = i.UpdateAt
                })
                .ToListAsync();
    }
}