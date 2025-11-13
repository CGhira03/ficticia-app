using Microsoft.AspNetCore.Mvc;

namespace Ficticia.Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Debe ingresar una contraseña.";
                return View();
            }

            if (password.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetString("RoleToken", "admin"); 
                HttpContext.Session.SetString("UserRole", "Admin");
                TempData["Message"] = "Sesión iniciada como ADMIN.";
                return RedirectToAction("Index", "Home");
            }

            if (password.Equals("consultor", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetString("RoleToken", "consultor"); 
                HttpContext.Session.SetString("UserRole", "Consultor");
                TempData["Message"] = "Sesión iniciada como CONSULTOR.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Contraseña inválida. Usa 'admin' o 'consultor'.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("RoleToken"); 
            HttpContext.Session.Remove("UserRole");
            TempData["Message"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Login");
        }
    }
}
