using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Repositories.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    /// <summary>
    /// Get conversation between two employees
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="receiverId"></param>
    /// <returns></returns>
    Task<IEnumerable<Message>> GetConversationAsync(Guid senderId, Guid receiverId);

    /// <summary>
    /// Get unread messages for an employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<IEnumerable<Message>> GetUnreadAsync(Guid employeeId);


    Task MarkAsReadAsync(IEnumerable<Guid> messageIds);
}