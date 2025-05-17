using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[Route("api/messages")]
[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("send")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        await _messageService.SendMessageAsync(
            request.SenderId,
            request.ReceiverId,
            request);
        return NoContent();
    }

    [HttpGet("conversation")]
    [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConversation(
        [FromQuery] Guid employee1Id,
        [FromQuery] Guid employee2Id,
        [FromQuery] PaginationRequest pagination)
    {
        var result = await _messageService.GetConversationAsync(employee1Id, employee2Id, pagination);
        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpGet("{employeeId}/unread")]
    [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnreadMessages(
        Guid employeeId,
        [FromQuery] PaginationRequest pagination)
    {
        var result = await _messageService.GetUnreadMessagesAsync(employeeId, pagination);
        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpPost("mark-read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkMessagesAsRead([FromBody] IEnumerable<Guid> messageIds)
    {
        await _messageService.MarkMessagesAsReadAsync(messageIds);
        return NoContent();
    }
}