using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] ChatMessage message)
        {
            try
            {
                var sentMessage = await _chatService.SendMessageAsync(message);
                return Ok(sentMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("conversation/{userId1}/{userId2}")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetConversation(Guid userId1, Guid userId2)
        {
            try
            {
                var messages = await _chatService.GetConversationAsync(userId1, userId2);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(Guid messageId)
        {
            try
            {
                await _chatService.MarkAsReadAsync(messageId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dietitians")]
        public async Task<ActionResult<IEnumerable<User>>> GetDietitians()
        {
            try
            {
                var dietitians = await _chatService.GetDietitiansAsync();
                return Ok(dietitians);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("clients/{dietitianId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetClients(Guid dietitianId)
        {
            try
            {
                var clients = await _chatService.GetClientsForDietitianAsync(dietitianId);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
