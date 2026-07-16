namespace RealEstateApp.Application.ViewModels.Agents;

public class AgentViewModel
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
    public int PropertyCount { get; set; }
    public bool IsActive { get; set; }
}