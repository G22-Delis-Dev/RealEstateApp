// Application/Interfaces/Identity/IAuthService.cs
namespace RealEstateApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<IEnumerable<UserSummary>> GetUsersByRoleAsync(string role);
    Task<UserSummary?> GetUserByIdInRoleAsync(string userId, string role);
    Task SetUserStatusAsync(string userId, bool isActive);
}

public class UserSummary
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; }
}