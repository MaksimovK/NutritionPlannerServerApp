using Microsoft.AspNetCore.Http;
using NutritionPlanner.Core.Models;
using NutritionPlanner.Application.Services.Interfaces;
using System.Security.Claims;

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
            if (context?.User?.Identity?.IsAuthenticated != true)
                return null;

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                return null;

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            if (!Enum.TryParse<Role>(roleClaim.Value, out var role))
                return null;

            return new User
            {
                Id = userId,
                Role = role
            };
        }
    }
}