using Neveria.Models.DTOs;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Services
{
    public interface IUserService
    {
        Task<List<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO?> GetByIdAsync(int id);
        Task<User?> GetRawByIdAsync(int id);       // para vistas Edit/Delete
        Task CreateAsync(User user);
        Task<bool> EditAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
