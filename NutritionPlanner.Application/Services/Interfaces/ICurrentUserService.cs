using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface ICurrentUserService
    {
        User? GetCurrentUser();
    }
}
