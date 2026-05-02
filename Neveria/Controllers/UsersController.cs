using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        // Roles los seguimos leyendo directo porque no tiene su propio service aún
        private readonly ICategoryService _categoryService;

        // Necesitamos el contexto solo para el SelectList de roles
        private readonly Neveria.Models.dbFreezeDream.DbFreezeDreamContext _context;

        public UsersController(IUserService userService, DbFreezeDreamContext context)
        {
            _userService = userService;
            _context     = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole");
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userService.GetByIdAsync(id.Value);
            if (user == null) return NotFound();
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
            ModelState.Remove("TagRoleNavigation");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                await _userService.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }

            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole", user.TagRole);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userService.GetRawByIdAsync(id.Value);
            if (user == null) return NotFound();
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
            if (id != user.TagUser) return NotFound();
            ModelState.Remove("TagRoleNavigation");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                var result = await _userService.EditAsync(id, user);
                if (!result) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TagRole"] = new SelectList(_context.Roles, "TagRole", "NameRole", user.TagRole);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userService.GetByIdAsync(id.Value);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
