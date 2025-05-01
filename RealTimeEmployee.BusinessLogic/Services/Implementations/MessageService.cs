using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendMessageRequest> _messageValidator;

    public MessageService(
        IUnitOfWork unitOfWork,
        IValidator<SendMessageRequest> messageValidator)
    {
        _unitOfWork = unitOfWork;
        _messageValidator = messageValidator;
    }

    public async Task SendMessageAsync(
        Guid senderId,
        Guid receiverId,
        SendMessageRequest request)
    {
        await _messageValidator.ValidateAndThrowAsync(request);

        await _unitOfWork.Messages.AddAsync(new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = request.Content,
            SentTime = DateTime.UtcNow,
            IsRead = false
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<MessageDto>> GetConversationAsync(
        Guid employee1Id,
        Guid employee2Id)
    {
        var messages = await _unitOfWork.Messages.GetConversationAsync(employee1Id, employee2Id);

        var employee1 = await _unitOfWork.Employees.GetByIdAsync(employee1Id);
        var employee2 = await _unitOfWork.Employees.GetByIdAsync(employee2Id);

        return messages.Select(m => new MessageDto(
            m.Id,
            m.SenderId,
            $"{employee1.FirstName} {employee1.LastName}",
            m.ReceiverId,
            $"{employee2.FirstName} {employee2.LastName}",
            m.Content,
            m.SentTime,
            m.IsRead));
    }

    public async Task<IEnumerable<MessageDto>> GetUnreadMessagesAsync(Guid employeeId)
    {
        var messages = await _unitOfWork.Messages.GetUnreadAsync(employeeId);
        var senders = new Dictionary<Guid, string>();

        var result = new List<MessageDto>();

        foreach (var message in messages)
        {
            if (!senders.TryGetValue(message.SenderId, out var senderName))
            {
                var sender = await _unitOfWork.Employees.GetByIdAsync(message.SenderId);
                senderName = $"{sender.FirstName} {sender.LastName}";
                senders[message.SenderId] = senderName;
            }

            result.Add(new MessageDto(
                message.Id,
                message.SenderId,
                senderName,
                message.ReceiverId,
                string.Empty, // Receiver name is not needed here
                message.Content,
                message.SentTime,
                message.IsRead));
        }

        return result;
    }

    public async Task MarkMessagesAsReadAsync(IEnumerable<Guid> messageIds)
    {
        await _unitOfWork.Messages.MarkAsReadAsync(messageIds);
        await _unitOfWork.SaveChangesAsync();
    }
}
