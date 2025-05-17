namespace RealTimeEmployee.BusinessLogic.Requests;

public record SendMessageRequest(Guid SenderId, Guid ReceiverId, string Content);