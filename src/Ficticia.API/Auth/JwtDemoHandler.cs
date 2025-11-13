using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ficticia.API.Auth
{
    public class JwtDemoHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public JwtDemoHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("No se envió encabezado Authorization."));

            var authHeader = Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Formato de token inválido."));

            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (token.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            if (token.Equals("consultor", StringComparison.OrdinalIgnoreCase))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Consultor"),
                    new Claim(ClaimTypes.Role, "Consultor")
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Token inválido."));
        }
    }
}
