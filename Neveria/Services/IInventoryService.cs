using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public interface IInventoryService
    {
        Task<List<InventarioDTO>> GetAllAsync();
        Task<List<InventarioDTO>> GetStockBajoAsync();
    }
}
