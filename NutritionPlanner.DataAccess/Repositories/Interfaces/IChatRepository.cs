using NutritionPlanner.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<ChatMessageEntity> CreateAsync(ChatMessageEntity message);
        Task<IEnumerable<ChatMessageEntity>> GetConversationAsync(Guid userId1, Guid userId2);
        Task MarkAsReadAsync(Guid messageId);
        Task<IEnumerable<UserEntity>> GetClientsForDietitianAsync(Guid dietitianId);
    }
}
