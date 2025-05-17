using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Data;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(RealTimeEmployeeDbContext context) : base(context) { }

    public async Task<IEnumerable<Message>> GetConversationAsync(Guid senderId, Guid receiverId)
        => await _dbSet
            .Where(m =>
                (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                (m.SenderId == receiverId && m.ReceiverId == senderId))
            .OrderBy(m => m.SentTime)
            .ToListAsync();

    public async Task<IEnumerable<Message>> GetUnreadAsync(Guid employeeId)
        => await _dbSet
            .Where(m => m.ReceiverId == employeeId && !m.IsRead)
            .ToListAsync();

    public async Task MarkAsReadAsync(IEnumerable<Guid> messageIds)
    {
        var messages = await _dbSet
            .Where(m => messageIds.Contains(m.Id))
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
            _dbSet.Update(message);
        }
    }
}