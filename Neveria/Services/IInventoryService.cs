using Neveria.Models.DTOs;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Services
{
    public interface IInventoryService
    {
        Task<List<InventarioDTO>> GetAllAsync();
        Task<List<InventarioDTO>> GetStockBajoAsync();
        Task<Inventory?> GetRawByIdAsync(int id);          // para vistas Edit/Delete
        Task<IQueryable<int>> GetProductIdsWithInventoryAsync();
        Task CreateAsync(Inventory inventory);
        Task<bool> EditAsync(int id, Inventory inventory);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
