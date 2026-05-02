using Neveria.Models.DTOs;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Services
{
    public interface ICategoryService
    {
        Task<List<CategoriesDTO>> GetAllAsync();
        Task<CategoriesDTO?> GetByIdAsync(int id);
        Task CreateAsync(Category category);
        Task<bool> EditAsync(int id, Category category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
