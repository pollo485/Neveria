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
                    TagInventory  = i.TagInventory,
                    NameProduct   = i.TagProductNavigation.NameProduct,
                    NameCategorie = i.TagProductNavigation.TagCategorieNavigation.NameCategorie,
                    StockQuantity = i.StockQuantity,
                    MinQuantity   = i.MinQuantity,
                    UpdateAt      = i.UpdateAt
                })
                .ToListAsync();

        public async Task<List<InventarioDTO>> GetStockBajoAsync() =>
            await _context.Inventories
                .Where(i => i.StockQuantity <= i.MinQuantity)
                .Include(i => i.TagProductNavigation)
                    .ThenInclude(p => p.TagCategorieNavigation)
                .Select(i => new InventarioDTO
                {
                    TagInventory  = i.TagInventory,
                    NameProduct   = i.TagProductNavigation.NameProduct,
                    NameCategorie = i.TagProductNavigation.TagCategorieNavigation.NameCategorie,
                    StockQuantity = i.StockQuantity,
                    MinQuantity   = i.MinQuantity,
                    UpdateAt      = i.UpdateAt
                })
                .ToListAsync();

        // Devuelve la entidad cruda para los formularios Edit/Delete
        public async Task<Inventory?> GetRawByIdAsync(int id) =>
            await _context.Inventories
                .Include(i => i.TagProductNavigation)
                .FirstOrDefaultAsync(i => i.TagInventory == id);

        // Devuelve los IDs de productos que YA tienen inventario (para el select de Create)
        public Task<IQueryable<int>> GetProductIdsWithInventoryAsync()
        {
            IQueryable<int> ids = _context.Inventories.Select(i => i.TagProduct);
            return Task.FromResult(ids);
        }

        public async Task CreateAsync(Inventory inventory)
        {
            inventory.UpdateAt = DateTime.Now;
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(int id, Inventory inventory)
        {
            if (id != inventory.TagInventory) return false;
            try
            {
                inventory.UpdateAt = DateTime.Now;
                _context.Update(inventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return false;
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Inventories.AnyAsync(i => i.TagInventory == id);
    }
}
