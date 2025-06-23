using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using System.Security.Cryptography;

namespace NutritionPlanner.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly EncryptionService _encryptionService;

        public ChatService(
            IChatRepository chatRepository,
            IUsersRepository usersRepository,
            IConfiguration config)
        {
            _chatRepository = chatRepository;
            _usersRepository = usersRepository;
            string base64Key = config["EncryptionSettings:Key"];
            byte[] key = Convert.FromBase64String(base64Key);
            _encryptionService = new EncryptionService(key);
        }

        public async Task<ChatMessage> SendMessageAsync(ChatMessage message)
        {
            try
            {
                (string encryptedContent, string iv) = _encryptionService.Encrypt(message.Content);

                var entity = new ChatMessageEntity
                {
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    Content = encryptedContent,
                    SentAt = DateTime.UtcNow,
                    IV = iv
                };

                var created = await _chatRepository.CreateAsync(entity);

                return new ChatMessage
                {
                    Id = created.Id,
                    SenderId = created.SenderId,
                    ReceiverId = created.ReceiverId,
                    Content = message.Content,
                    SentAt = created.SentAt,
                    IsRead = created.IsRead,
                    IV = created.IV
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            try
            {
                var messages = await _chatRepository.GetConversationAsync(userId1, userId2);

                var result = new List<ChatMessage>();
                int decryptedCount = 0;
                int errorCount = 0;

                foreach (var m in messages)
                {
                    try
                    {
                        string decryptedContent = _encryptionService.Decrypt(m.Content, m.IV);
                        decryptedCount++;

                        result.Add(new ChatMessage
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            ReceiverId = m.ReceiverId,
                            Content = decryptedContent,
                            SentAt = m.SentAt,
                            IsRead = m.IsRead,
                            IV = m.IV
                        });
                    }
                    catch (CryptographicException ex)
                    {
                        errorCount++;

                        result.Add(new ChatMessage
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            ReceiverId = m.ReceiverId,
                            Content = "[не удалось расшифровать]",
                            SentAt = m.SentAt,
                            IsRead = m.IsRead,
                            IV = m.IV
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
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
