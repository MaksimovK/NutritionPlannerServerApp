using Microsoft.AspNetCore.Http;
using NutritionPlanner.Core.Models;
using NutritionPlanner.Application.Services.Interfaces;

namespace NutritionPlanner.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User? GetCurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.Request?.Headers == null) return null;

            // Получаем заголовки
            var userIdHeader = context.Request.Headers["X-User-Id"].FirstOrDefault();
            var tokenHeader = context.Request.Headers["X-Auth-Token"].FirstOrDefault();

            if (!string.IsNullOrEmpty(userIdHeader) && Guid.TryParse(userIdHeader, out var userId))
            {
                // В реальном приложении здесь должна быть проверка токена
                return new User
                {
                    Id = userId,
                    Role = Enum.TryParse<Role>(context.Request.Headers["X-User-Role"].FirstOrDefault(), out var role)
                        ? role
                        : Role.User
                };
            }

            return null;
        }
    }
}