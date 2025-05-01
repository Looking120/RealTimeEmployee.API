namespace RealTimeEmployee.DataAccess.Entitites;

public class Message : BaseEntity
{
    public string Content { get; set; }

    public DateTime SentTime { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; } = false;

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public Employee Sender { get; set; }

    public Employee Receiver { get; set; }
}