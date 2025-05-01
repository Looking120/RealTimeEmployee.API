namespace RealTimeEmployee.BusinessLogic.Dtos;

public record MessageDto(
    Guid Id,
    Guid SenderId,
    string SenderName,
    Guid ReceiverId,
    string ReceiverName,
    string Content,
    DateTime SentTime,
    bool IsRead);