using RealEstateApp.Application.DTOs.Account;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string channel);
    Task<string> RegisterClientAsync(RegisterDeveloperRequestDto request, string origin);
    Task<string> RegisterAgentAsync(RegisterDeveloperRequestDto request);
    Task<string> ConfirmEmailAsync(string userId, string token);
    Task<string> CreateAdministratorAsync(RegisterAdministratorRequestDto request, string currentAdminId);
    Task DeactivateAdministratorAsync(string adminIdToDeactivate, string currentAdminId);
    Task<string> CreateDeveloperAsync(RegisterDeveloperRequestDto request, string origin);
}
