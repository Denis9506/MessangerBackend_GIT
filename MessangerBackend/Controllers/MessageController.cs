﻿using MessangerBackend.Core.Models;
using MessangerBackend.DTOs;
using MessangerBackend.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessangerBackend.Controllers
{
    [ApiController]
    [Route("message")]
    public class MessageController : Controller
    {
        private readonly MessangerContext _context;

        public MessageController(MessangerContext context)
        {
            _context = context;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<PrivateChatDTO>> CreatePrivateChat(PrivateChatDTO privateChatDto)
        {
            var users = _context.Users.Where(x => privateChatDto.UsersIds.Contains(x.Id)).ToList();
            var privateChat = new PrivateChat()
            {
                Users = users,
                CreatedAt = DateTime.UtcNow
            };
            _context.Add(privateChat);
            await _context.SaveChangesAsync();

            return Ok(privateChatDto);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SendMessage(MessageDTO messageDto)
        {
            var sender = _context.Users.Single(x => x.Id == messageDto.SenderId);
            var chat = _context.PrivateChats.Single(x => x.Id == messageDto.ChatId);
            var message = new Message()
            {
                Content = messageDto.Text,
                Sender = sender,
                Chat = chat,
                SentAt = DateTime.UtcNow
            };

            _context.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet("sent/{userId}")]
        public ActionResult<List<MessageDTO>> GetSentMessagesByUserId(int userId)
        {
            var messages = _context.Messages
                .Where(m => m.Sender.Id == userId)
                .ToList();

            return Ok(messages);
        }

        [HttpGet("received/{userId}")]
        public ActionResult<List<MessageDTO>> GetReceivedMessagesByUserId(int userId)
        {
            var messages = _context.Messages
                .Where(m => m.Chat.Users.Any(u => u.Id == userId) && m.Sender.Id != userId)
                .ToList();

            return Ok(messages);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<IEnumerable<ShowMessangerDTO>>> GetMessages(int chatId) {
            var chat = await _context.PrivateChats.Include(x=>x.Messages).ThenInclude(x=>x.Sender).FirstAsync(x=>x.Id==chatId);
            var messages = chat.Messages;

            return Ok( messages.Select(x => new ShowMessangerDTO() { 
            
            }));
        }
    }
}
