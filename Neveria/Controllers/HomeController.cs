using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neveria.Models;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;
using Neveria.Services;

namespace Neveria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService         _productService;
        private readonly ISaleService            _saleService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            ISaleService saleService)
        {
            _logger         = logger;
            _productService = productService;
            _saleService    = saleService;
        }

        // GET: /Home/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string contrasena)
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(contrasena))
                return RedirectToAction("Inicio");

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        // GET: /Home/Registro
        [HttpGet]
        public IActionResult Registro() => View();

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
                return RedirectToAction("Login");
            }
            ViewBag.Error = "Por favor llena todos los campos.";
            return View();
        }

        // GET: /Home/Inicio
        public IActionResult Inicio() => View();

        // GET: /Home/Productos
        public IActionResult Productos() => View();

        // GET: /Home/DetallesProducto
        public async Task<IActionResult> DetallesProducto(int? id)
        {
            var productos = await _productService.GetAllActiveAsync();

            var seleccionado = id.HasValue
                ? productos.FirstOrDefault(p => p.TagProduct == id)
                : productos.FirstOrDefault();

            ViewBag.ProductoSeleccionado = seleccionado;
            return View(productos);
        }

        // GET: /Home/Ventas
        public IActionResult Ventas() => View();

        // GET: /Home/Graficos
        public async Task<IActionResult> Graficos()
        {
            var datos = await _saleService.GetVentasPorProductoAsync();
            return View(datos);
        }

        // GET: /Home/Privacy
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
