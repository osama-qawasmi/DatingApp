using API.DTOs;
using API.Enitities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessages(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m=>m.MessageSent).AsQueryable();
            query = messageParams.Container switch
            {
                    "Inbox" => query.Where(m => m.RecipientUsername == messageParams.Username && m.RecipientDeleted == false),
                    "Outbox" => query.Where(m => m.SenderUsername == messageParams.Username && m.SenderDeleted == false),
                    _ => query.Where(m => m.RecipientUsername == messageParams.Username && m.RecipientDeleted == false && m.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
            .Include(s => s.Sender).ThenInclude(s => s.Photos)
            .Include(r => r.Recipient).ThenInclude(r => r.Photos)
            .Where
            (
                m => 
                (m.RecipientUsername == currentUsername && m.SenderUsername == recipientUsername && m.RecipientDeleted == false)
                ||
                (m.RecipientUsername == recipientUsername && m.SenderUsername == currentUsername && m.SenderDeleted == false)
            ).OrderBy(m => m.MessageSent)
            .ToListAsync();

            var unreadMessages = messages.Where(m => m.RecipientUsername == currentUsername && m.DateRead == null).ToList();

            if(unreadMessages.Any())
            {
                foreach(var unreadMessage in unreadMessages)
                {
                    unreadMessage.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}