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
                    TagUser   = u.TagUser,
                    Name      = u.Name,
                    UserName  = u.UserName,
                    Email     = u.Email,
                    NombreRol = u.TagRoleNavigation.NameRole,
                    CreatedAt = u.CreatedAt,
                    IsActive  = u.IsActive
                })
                .ToListAsync();

        public async Task<UsuarioDTO?> GetByIdAsync(int id) =>
            await _context.Users
                .Where(u => u.TagUser == id)
                .Include(u => u.TagRoleNavigation)
                .Select(u => new UsuarioDTO
                {
                    TagUser   = u.TagUser,
                    Name      = u.Name,
                    UserName  = u.UserName,
                    Email     = u.Email,
                    NombreRol = u.TagRoleNavigation.NameRole,
                    CreatedAt = u.CreatedAt,
                    IsActive  = u.IsActive
                })
                .FirstOrDefaultAsync();

        // Devuelve la entidad cruda para formularios que necesitan todos los campos
        public async Task<User?> GetRawByIdAsync(int id) =>
            await _context.Users
                .Include(u => u.TagRoleNavigation)
                .FirstOrDefaultAsync(u => u.TagUser == id);

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Sincroniza la tabla Employees automáticamente
            await SyncEmpleadoAsync(user.TagUser, user.TagRole);
        }

        public async Task<bool> EditAsync(int id, User user)
        {
            if (id != user.TagUser) return false;
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                // Sincroniza la tabla Employees automáticamente
                await SyncEmpleadoAsync(user.TagUser, user.TagRole);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            // Elimina de Employees primero para evitar error de FK
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.TagUser == id);
            if (emp != null) _context.Employees.Remove(emp);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Users.AnyAsync(u => u.TagUser == id);

        // -------------------------------------------------------
        // Método privado: sincroniza la tabla Employees
        // Si el usuario NO es Cliente → lo agrega como empleado
        // Si el usuario ES Cliente   → lo elimina de empleados
        // -------------------------------------------------------
        private async Task SyncEmpleadoAsync(int tagUser, int tagRole)
        {
            var rolCliente = await _context.Roles
                .FirstOrDefaultAsync(r => r.NameRole.ToLower() == "cliente");

            bool esCliente = rolCliente != null && tagRole == rolCliente.TagRole;

            if (!esCliente)
            {
                bool yaEsEmpleado = await _context.Employees.AnyAsync(e => e.TagUser == tagUser);
                if (!yaEsEmpleado)
                {
                    _context.Employees.Add(new Employee { TagUser = tagUser });
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.TagUser == tagUser);
                if (emp != null)
                {
                    _context.Employees.Remove(emp);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
