using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public interface IProductService
    {
        Task<List<ProductoDetalleDTO>> GetAllActiveAsync();
        Task<ProductoDetalleDTO?> GetByIdAsync(int id);
    }
}
