using RealEstateApp.Application.DTOs.Account;
namespace RealEstateApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<string> RegisterDeveloperAsync(RegisterDeveloperRequestDto request, string origin);
    Task<string> RegisterAdministratorAsync(RegisterAdministratorRequestDto request, string origin);
    Task<string> ConfirmEmailAsync(string userId, string token);
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
