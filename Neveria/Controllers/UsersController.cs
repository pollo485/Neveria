using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;

namespace Neveria.Controllers
{
    public class UsersController : Controller
    {
        private readonly DbFreezeDreamContext _context;

        public UsersController(DbFreezeDreamContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------
        // Método auxiliar: sincroniza la tabla Employees
        // Si el usuario NO es Cliente → lo agrega como empleado
        // Si el usuario ES Cliente   → lo elimina de empleados
        // -------------------------------------------------------
        private async Task SyncEmpleado(int tagUser, int tagRole)
        {
            var rolCliente = await _context.Roles
                .FirstOrDefaultAsync(r => r.NameRole.ToLower() == "cliente");

            bool esCliente = rolCliente != null && tagRole == rolCliente.TagRole;

            if (!esCliente)
            {
                // Si no es cliente y aún no es empleado → agregar
                bool yaEsEmpleado = await _context.Employees
                    .AnyAsync(e => e.TagUser == tagUser);

                if (!yaEsEmpleado)
                {
                    _context.Employees.Add(new Employee { TagUser = tagUser });
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Si cambió a cliente → remover de empleados
                var emp = await _context.Employees
                    .FirstOrDefaultAsync(e => e.TagUser == tagUser);

                if (emp != null)
                {
                    _context.Employees.Remove(emp);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole");
            var dbFreezeDreamContext = _context.Users.Include(u => u.TagRoleNavigation);
            return View(await dbFreezeDreamContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Include(u => u.TagRoleNavigation)
                .FirstOrDefaultAsync(m => m.TagUser == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TagUser,TagRole,Name,UserName,Email,Password,CreatedAt,IsActive")] User user)
        {
            // Quitar validaciones que causan problemas
            ModelState.Remove("TagRoleNavigation");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                // Sincronizar tabla Employees automáticamente
                await SyncEmpleado(user.TagUser, user.TagRole);

                return RedirectToAction(nameof(Index));
            }

            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole", user.TagRole);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole", user.TagRole);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("TagUser,TagRole,Name,UserName,Email,Password,CreatedAt,IsActive")] User user)
        {
            if (id != user.TagUser)
                return NotFound();

            ModelState.Remove("TagRoleNavigation");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    // Sincronizar tabla Employees automáticamente
                    await SyncEmpleado(user.TagUser, user.TagRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.TagUser))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole", user.TagRole);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Include(u => u.TagRoleNavigation)
                .FirstOrDefaultAsync(m => m.TagUser == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                // Eliminar de Employees primero si existe (evitar error de FK)
                var emp = await _context.Employees
                    .FirstOrDefaultAsync(e => e.TagUser == id);
                if (emp != null)
                    _context.Employees.Remove(emp);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.TagUser == id);
        }
    }
}