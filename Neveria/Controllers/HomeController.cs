using Microsoft.AspNetCore.Mvc;
using Neveria.Models;
using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

namespace Neveria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbFreezeDreamContext _context;

        public HomeController(ILogger<HomeController> logger, DbFreezeDreamContext context)
        {
            _logger = logger;
            _context = context;

        }

        // GET: /Home/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string contrasena)
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(contrasena))
            {
                return RedirectToAction("Inicio");
            }
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        // GET: /Home/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: /Home/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(string nombre, string usuario, string correo, string contrasena, string confirmar)
        {
            if (contrasena != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(usuario)
                && !string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(contrasena))
            {
                // TODO: guardar usuario en base de datos
                return RedirectToAction("Login");
            }
            ViewBag.Error = "Por favor llena todos los campos.";
            return View();
        }

        // GET: /Home/Inicio
        public IActionResult Inicio()
        {
            return View();
        }

        // GET: /Home/Productos
        public IActionResult Productos()
        {
            return View();
        }

        // GET: /Home/DetallesProducto
        public async Task<IActionResult> DetallesProducto(int? id)
        {
            // Carga todos los productos con su categoría e inventario
            var productos = await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.TagCategorieNavigation)
                .Include(p => p.Inventory)
                .Select(p => new ProductoDetalleDTO
                {
                    TagProduct = p.TagProduct,
                    NameProduct = p.NameProduct,
                    UnitPrice = p.UnitPrice,
                    DescriptionProduct = p.DescriptionProduct,
                    NameCategorie = p.TagCategorieNavigation.NameCategorie,
                    TagCategorie = p.TagCategorie,
                    StockQuantity = p.Inventory != null ? p.Inventory.StockQuantity : 0,
                    StockBajo = p.Inventory != null && p.Inventory.StockQuantity <= p.Inventory.MinQuantity
                })
                .ToListAsync();

            // Si viene un id, selecciona ese producto; si no, el primero
            var seleccionado = id.HasValue
                ? productos.FirstOrDefault(p => p.TagProduct == id)
                : productos.FirstOrDefault();

            ViewBag.ProductoSeleccionado = seleccionado;
            return View(productos);
        }
        public IActionResult Ventas() => View();
        public IActionResult Graficos() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
