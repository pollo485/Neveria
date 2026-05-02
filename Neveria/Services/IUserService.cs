using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public interface IUserService
    {
        Task<List<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO?> GetByIdAsync(int id);
    }
}
