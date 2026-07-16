namespace RealEstateApp.Application.ViewModels.Messages;

public class MessageViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public string SenderId { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string AgentId { get; set; } = null!;
}