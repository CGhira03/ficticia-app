using Microsoft.AspNetCore.Mvc;
using Ficticia.Web.Models;
using Ficticia.Web.Services;

namespace Ficticia.Web.Controllers
{
    public class PersonsController : Controller
    {
        private readonly PersonService _service;

        public PersonsController(PersonService service)
        {
            _service = service;
        }

        // Obtenemos el rol actual desde la sesión
        private string? GetRole() => HttpContext.Session.GetString("UserRole");
        private bool IsAdmin() => GetRole()?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

        public async Task<IActionResult> Index(string? name, bool? isActive, int? minAge, int? maxAge)
        {
            var model = new PersonFilterViewModel();

            try
            {
                var persons = await _service.GetAllAsync();

                if (!string.IsNullOrEmpty(name))
                    persons = persons.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (isActive != null)
                    persons = persons.Where(p => p.IsActive == isActive).ToList();

                if (minAge != null)
                    persons = persons.Where(p => p.Age >= minAge).ToList();

                if (maxAge != null)
                    persons = persons.Where(p => p.Age <= maxAge).ToList();

                model.Persons = persons;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ApiError = "No se pudieron cargar las personas: " + ex.Message;
            }

            model.Name = name;
            model.IsActive = isActive;
            model.MinAge = minAge;
            model.MaxAge = maxAge;

            // Rol actual (admin o consultor)
            var role = GetRole();
            model.Role = role?.ToLowerInvariant() ?? string.Empty;

            return View(model);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                TempData["Message"] = "⚠️ Solo los administradores pueden crear personas.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonDto model)
        {
            if (!IsAdmin())
            {
                TempData["Message"] = "⚠️ No tienes permisos para crear.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) return View(model);

            model.Attributes = model.Attributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Type) || !string.IsNullOrWhiteSpace(a.Value))
                .ToList();

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                TempData["Message"] = "⚠️ Solo los administradores pueden editar personas.";
                return RedirectToAction("Index", "Home");
            }

            var person = await _service.GetByIdAsync(id);

            person.Attributes ??= new List<PersonAttribute>();

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonDto model)
        {
            if (!IsAdmin())
            {
                TempData["Message"] = "⚠️ No tienes permisos para editar.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) return View(model);

            model.Attributes = model.Attributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Type) || !string.IsNullOrWhiteSpace(a.Value))
                .ToList();

            await _service.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                TempData["Message"] = "⚠️ Solo los administradores pueden eliminar personas.";
                return RedirectToAction("Index", "Home");
            }

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
