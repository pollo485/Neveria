using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public class UserService : IUserService
    {
        private readonly DbFreezeDreamContext _context;
        public UserService(DbFreezeDreamContext context) => _context = context;

        public async Task<List<UsuarioDTO>> GetAllAsync() =>
            await _context.Users
                .Include(u => u.TagRoleNavigation)
                .Select(u => new UsuarioDTO
                {
                    TagUser = u.TagUser,
                    Name = u.Name,
                    UserName = u.UserName,
                    Email = u.Email,
                    NombreRol = u.TagRoleNavigation.NameRole,
                    CreatedAt = u.CreatedAt,
                    IsActive = u.IsActive
                })
                .ToListAsync();

        public async Task<UsuarioDTO?> GetByIdAsync(int id) =>
            await _context.Users
                .Where(u => u.TagUser == id)
                .Include(u => u.TagRoleNavigation)
                .Select(u => new UsuarioDTO
                {
                    TagUser = u.TagUser,
                    Name = u.Name,
                    UserName = u.UserName,
                    Email = u.Email,
                    NombreRol = u.TagRoleNavigation.NameRole,
                    CreatedAt = u.CreatedAt,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync();
    }
}