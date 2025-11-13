using System.Net.Http.Json;
using Ficticia.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Ficticia.Web.Services
{
    public class PersonService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonService(HttpClient http, IConfiguration config, IHttpContextAccessor accessor)
        {
            _http = http;
            _config = config;
            _httpContextAccessor = accessor;

            // Base URL de la API
            _http.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]);
        }

        private void AddAuthHeader()
        {
            // Elimina encabezados previos si los hubiera
            _http.DefaultRequestHeaders.Authorization = null;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("RoleToken");

            if (!string.IsNullOrEmpty(token))
            {
                // Formato correcto para tu JwtDemoHandler
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[HTTP] ✅ Token aplicado: Bearer {token}");
            }
            else
            {
                Console.WriteLine("[HTTP] ⚠️ No hay token en sesión");
            }
        }

        // -------------------- GET --------------------
        public async Task<List<PersonDto>> GetAllAsync()
        {
            AddAuthHeader();
            return await _http.GetFromJsonAsync<List<PersonDto>>("api/Persons") ?? new List<PersonDto>();
        }

        public async Task<List<PersonDto>> GetFilteredAsync(string? name = null, bool? isActive = null, int? minAge = null, int? maxAge = null)
        {
            AddAuthHeader();
            var query = new List<string>();

            if (!string.IsNullOrEmpty(name)) query.Add($"name={Uri.EscapeDataString(name)}");
            if (isActive.HasValue) query.Add($"isActive={isActive.Value.ToString().ToLower()}");
            if (minAge.HasValue) query.Add($"minAge={minAge.Value}");
            if (maxAge.HasValue) query.Add($"maxAge={maxAge.Value}");

            var url = "api/Persons";
            if (query.Any()) url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<List<PersonDto>>(url) ?? new List<PersonDto>();
        }

        public async Task<PersonDto?> GetByIdAsync(int id)
        {
            AddAuthHeader();
            return await _http.GetFromJsonAsync<PersonDto>($"api/Persons/{id}");
        }

        // -------------------- CREATE --------------------
        public async Task CreateAsync(PersonDto person)
        {
            AddAuthHeader(); // aplica token si existe

            var payload = new
            {
                person.FullName,
                person.Identification,
                person.Age,
                person.Gender,
                person.IsActive,
                Attributes = person.Attributes?.Select(a => new
                {
                    a.Type,
                    a.Value
                }).ToList()
            };

            var response = await _http.PostAsJsonAsync("api/Persons", payload);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al crear persona: {(int)response.StatusCode} {response.ReasonPhrase} - {content}");
            }
        }

        // -------------------- UPDATE --------------------
        public async Task UpdateAsync(PersonDto person)
        {
            AddAuthHeader(); // aplica token si existe

            var payload = new
            {
                person.FullName,
                person.Identification,
                person.Age,
                person.Gender,
                person.IsActive,
                Attributes = person.Attributes?.Select(a => new
                {
                    a.Type,
                    a.Value
                }).ToList()
            };

            var response = await _http.PutAsJsonAsync($"api/Persons/{person.Id}", payload);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al actualizar persona: {(int)response.StatusCode} {response.ReasonPhrase} - {content}");
            }
        }

        // -------------------- DELETE --------------------
        public async Task DeleteAsync(int id)
        {
            AddAuthHeader();
            var response = await _http.DeleteAsync($"api/Persons/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
