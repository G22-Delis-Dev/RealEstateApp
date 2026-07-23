namespace RealEstateApp.Application.ViewModels.Agents;

public class AgentListItemViewModel
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public int PropertyCount { get; set; }
    public bool IsActive { get; set; }
}