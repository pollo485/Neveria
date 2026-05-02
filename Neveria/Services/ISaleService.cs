using Neveria.Models.DTOs;
using Neveria.Models.dbFreezeDream;
using Microsoft.AspNetCore.Http;

namespace Neveria.Services
{
    public interface ISaleService
    {
        Task<List<VentaResumenDTO>> GetAllAsync();
        Task<VentaResumenDTO?> GetByIdAsync(int id);
        Task<List<VentasPorProductoDTO>> GetVentasPorProductoAsync();
        Task<List<object>> GetEmpleadosSelectAsync();   // para el select del formulario
        Task<List<object>> GetProductosActivosAsync();  // para el carrito JSON
        Task<bool> CreateAsync(IFormCollection form);
        Task<bool> DeleteAsync(int id);
    }
}
