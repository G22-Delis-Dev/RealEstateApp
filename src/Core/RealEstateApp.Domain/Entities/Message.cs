using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.Entities;

public class Message : AuditableEntity
{
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }

    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public string ClientId { get; set; } = null!;
    public string AgentId { get; set; } = null!;
    public string SenderId { get; set; } = null!;
}