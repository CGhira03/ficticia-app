using Ficticia.Web.Models;
using Ficticia.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ficticia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PersonService _personService;

        public HomeController(ILogger<HomeController> logger, PersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        public async Task<IActionResult> Index(string? name, bool? isActive, int? minAge, int? maxAge)
        {
            var filterModel = new PersonFilterViewModel
            {
                Name = name,
                IsActive = isActive,
                MinAge = minAge,
                MaxAge = maxAge
            };

            try
            {
                var persons = await _personService.GetAllAsync();

                if (!string.IsNullOrEmpty(name))
                    persons = persons.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (isActive != null)
                    persons = persons.Where(p => p.IsActive == isActive).ToList();

                if (minAge != null)
                    persons = persons.Where(p => p.Age >= minAge).ToList();

                if (maxAge != null)
                    persons = persons.Where(p => p.Age <= maxAge).ToList();

                filterModel.Persons = persons;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Error al obtener personas: {Message}", ex.Message);
                ViewBag.ApiError = "No se pudieron cargar las personas (requiere iniciar sesión o token inválido).";
            }

            var token = HttpContext.Session.GetString("RoleToken");
            filterModel.Role = token?.ToLowerInvariant();

            return View(filterModel);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
