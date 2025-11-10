using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Ficticia.API.Auth
{
    public class JwtDemoHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public JwtDemoHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Leer encabezado Authorization
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
                return Task.FromResult(AuthenticateResult.Fail("No token provided"));

            // Token simulado: "Bearer admin" o "Bearer consultor"
            var token = authHeader.Replace("Bearer ", "").Trim();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, token)
            };

            if (token.Equals("admin", StringComparison.OrdinalIgnoreCase))
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            else if (token.Equals("consultor", StringComparison.OrdinalIgnoreCase))
                claims.Add(new Claim(ClaimTypes.Role, "Consultor"));
            else
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
