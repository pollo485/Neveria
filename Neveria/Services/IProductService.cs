using Neveria.Models.DTOs;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Services
{
    public interface IProductService
    {
        Task<List<ProductoDetalleDTO>> GetAllActiveAsync();
        Task<ProductoDetalleDTO?> GetByIdAsync(int id);
        Task<List<Product>> GetAllRawAsync();        // para las vistas CRUD que necesitan la entidad
        Task CreateAsync(Product product);
        Task<bool> EditAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
