using Microsoft.AspNetCore.Mvc;

namespace Ficticia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] string password)
        {
            if (password == "admin" || password == "consultor")
                return Ok(new { token = password.ToLower() });

            return Unauthorized("Contraseña inválida. Use 'admin' o 'consultor'.");
        }
    }

}
