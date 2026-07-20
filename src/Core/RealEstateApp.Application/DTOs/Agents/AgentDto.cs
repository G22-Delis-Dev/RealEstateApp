namespace RealEstateApp.Application.DTOs.Agents;

public class AgentDto
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int PropertyCount { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}