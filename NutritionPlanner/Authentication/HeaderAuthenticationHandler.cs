using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NutritionPlanner.Core.Models; // Ваш enum Role

namespace NutritionPlanner.API.Authentication
{
    // Наследуем AuthenticationSchemeOptions, т. е. базовая схема без доп. настроек
    public class HeaderAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HeaderAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Проверяем наличие заголовков
            if (!Request.Headers.ContainsKey("X-User-Id") ||
                !Request.Headers.ContainsKey("X-User-Role"))
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "Необходимо передать X-User-Id и X-User-Role"));
            }

            var userIdValue = Request.Headers["X-User-Id"].FirstOrDefault();
            var roleValue = Request.Headers["X-User-Role"].FirstOrDefault();

            // Валидируем Guid
            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "X-User-Id не является корректным Guid"));
            }

            // Валидируем enum Role
            if (!Enum.TryParse<Role>(roleValue, true, out var roleEnum))
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "X-User-Role не совпадает с допустимым Role (Admin/User)"));
            }

            // Собираем клаймы: NameIdentifier = userId, Role = roleEnum.ToString()
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, roleEnum.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
