using NutritionPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatMessage> SendMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid userId1, Guid userId2);
        Task MarkAsReadAsync(Guid messageId);
        Task<IEnumerable<User>> GetDietitiansAsync();
        Task<IEnumerable<User>> GetClientsForDietitianAsync(Guid dietitianId);
    }
}
