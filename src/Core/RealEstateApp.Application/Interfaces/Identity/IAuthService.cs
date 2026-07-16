using RealEstateApp.Application.DTOs.Account;

namespace RealEstateApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<string> RegisterDeveloperAsync(RegisterDeveloperRequestDto request, string origin);
    Task<string> RegisterAdministratorAsync(RegisterAdministratorRequestDto request, string origin);
    Task<string> ConfirmEmailAsync(string userId, string token);
}
