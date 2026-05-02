using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public interface ISaleService
    {
        Task<List<VentaResumenDTO>> GetAllAsync();
        Task<VentaResumenDTO?> GetByIdAsync(int id);
        Task<List<VentasPorProductoDTO>> GetVentasPorProductoAsync();
    }
}
