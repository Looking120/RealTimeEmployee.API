using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(Guid senderId, Guid receiverId, SendMessageRequest request);

    Task<IEnumerable<MessageDto>> GetConversationAsync(Guid employee1Id, Guid employee2Id);

    Task<IEnumerable<MessageDto>> GetUnreadMessagesAsync(Guid employeeId);

    Task MarkMessagesAsReadAsync(IEnumerable<Guid> messageIds);
}
