using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUsersRepository _usersRepository;

        public ChatService(IChatRepository chatRepository, IUsersRepository usersRepository)
        {
            _chatRepository = chatRepository;
            _usersRepository = usersRepository;
        }

        public async Task<ChatMessage> SendMessageAsync(ChatMessage message)
        {
            var entity = new ChatMessageEntity
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = DateTime.UtcNow
            };

            var created = await _chatRepository.CreateAsync(entity);
            return new ChatMessage
            {
                Id = created.Id,
                SenderId = created.SenderId,
                ReceiverId = created.ReceiverId,
                Content = created.Content,
                SentAt = created.SentAt,
                IsRead = created.IsRead
            };
        }

        public async Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            var messages = await _chatRepository.GetConversationAsync(userId1, userId2);
            return messages.Select(m => new ChatMessage
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            });
        }

        public async Task MarkAsReadAsync(Guid messageId)
        {
            await _chatRepository.MarkAsReadAsync(messageId);
        }

        public async Task<IEnumerable<User>> GetDietitiansAsync()
        {
            var dietitians = await _usersRepository.GetByRoleAsync(Role.Dietitian);
            return dietitians.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            });
        }

        public async Task<IEnumerable<User>> GetClientsForDietitianAsync(Guid dietitianId)
        {
            var users = await _chatRepository.GetClientsForDietitianAsync(dietitianId);
            return users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            });
        }
    }
}
