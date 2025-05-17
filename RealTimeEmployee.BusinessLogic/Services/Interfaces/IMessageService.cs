using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(Guid senderId, Guid receiverId, SendMessageRequest request);
    Task<PaginatedResult<MessageDto>> GetConversationAsync(Guid employee1Id, Guid employee2Id, PaginationRequest pagination);
    Task<IEnumerable<MessageDto>> GetUnreadMessagesAsync(Guid employeeId);
    Task<PaginatedResult<MessageDto>> GetUnreadMessagesAsync(Guid employeeId, PaginationRequest pagination);
    Task MarkMessagesAsReadAsync(IEnumerable<Guid> messageIds);
}
