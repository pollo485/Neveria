using Microsoft.AspNetCore.Mvc;
using Neveria.Models;

namespace Neveria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult DetallesProducto()
        {
            return View();
        }

        // GET: /Home/Ventas
        public IActionResult Ventas()
        {
            return View();
        }

        // GET: /Home/Graficos
        public IActionResult Graficos()
        {
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

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
