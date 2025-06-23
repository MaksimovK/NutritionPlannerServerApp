using Microsoft.EntityFrameworkCore;
using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly NutritionPlannerDbContext _context;
        public ChatRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMessageEntity> CreateAsync(ChatMessageEntity message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<ChatMessageEntity>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            return await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(Guid messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserEntity>> GetClientsForDietitianAsync(Guid dietitianId)
        {
            var senders = await _context.ChatMessages
                .Where(m => m.ReceiverId == dietitianId)
                .Select(m => m.Sender)
                .Distinct()
                .ToListAsync();

            var receivers = await _context.ChatMessages
                .Where(m => m.SenderId == dietitianId)
                .Select(m => m.Receiver)
                .Distinct()
                .ToListAsync();

            return senders.Concat(receivers)
                .GroupBy(u => u.Id)
                .Select(g => g.First())
                .ToList();
        }
    }
}
