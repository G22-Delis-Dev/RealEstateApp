using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Account;

namespace RealEstateApp.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAuthService _authService;

    public AccountService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string channel)
    {
        var response = await _authService.LoginAsync(request);

        // Validar acceso por canal
        ValidateChannelAccess(response.Roles, channel);

        return response;
    }

    public async Task<string> RegisterClientAsync(RegisterDeveloperRequestDto request, string origin)
    {
        // Los clientes quedan Inactivos y reciben correo de activación
        return await _authService.RegisterDeveloperAsync(request, origin);
    }

    public async Task<string> RegisterAgentAsync(RegisterDeveloperRequestDto request)
    {
        // Los agentes quedan Inactivos y NO reciben correo de activación
        return await _authService.RegisterAgentAsync(request);
    }

    public async Task<string> ConfirmEmailAsync(string userId, string token)
    {
        return await _authService.ConfirmEmailAsync(userId, token);
    }

    public async Task<string> CreateAdministratorAsync(RegisterAdministratorRequestDto request, string currentAdminId)
    {
        BusinessRuleValidator.CheckRules(
            new PasswordsMustMatchRule(request.Password, request.ConfirmPassword),
            new EmailMustBeUniqueRule(await _authService.EmailExistsAsync(request.Email)),
            new UsernameMustBeUniqueRule(await _authService.UsernameExistsAsync(request.UserName)),
            new CedulaMustBeUniqueRule(await _authService.CedulaExistsAsync(request.IdCard))
        );

        return await _authService.RegisterAdministratorAsync(request, string.Empty);
    }

    public async Task DeactivateAdministratorAsync(string adminIdToDeactivate, string currentAdminId)
    {
        BusinessRuleValidator.CheckRules(
            new AdminCannotSelfModifyRule(currentAdminId, adminIdToDeactivate),
            new LastActiveAdminCannotBeDeactivatedRule(await _authService.CountActiveAdminUsersAsync(), true)
        );

        await _authService.SetUserStatusAsync(adminIdToDeactivate, false);
    }

    public async Task<string> CreateDeveloperAsync(RegisterDeveloperRequestDto request, string origin)
    {
        BusinessRuleValidator.CheckRules(
            new PasswordsMustMatchRule(request.Password, request.ConfirmPassword),
            new EmailMustBeUniqueRule(await _authService.EmailExistsAsync(request.Email)),
            new UsernameMustBeUniqueRule(await _authService.UsernameExistsAsync(request.UserName))
        );

        if (!string.IsNullOrWhiteSpace(request.IdCard))
        {
            BusinessRuleValidator.CheckRule(new CedulaMustBeUniqueRule(await _authService.CedulaExistsAsync(request.IdCard)));
        }

        return await _authService.RegisterDeveloperAsync(request, origin);
    }

    /// <summary>
    /// Valida que el usuario tenga permiso de acceder por el canal solicitado.
    /// - WebApp: solo Client y Agent pueden loguearse.
    /// - API: solo Admin y Developer pueden autenticarse.
    /// </summary>
    private static void ValidateChannelAccess(List<string> roles, string channel)
    {
        switch (channel.ToLower())
        {
            case "webapp":
                if (roles.Any(r => r == "Developer" || r == "Admin"))
                    throw new ForbiddenAccessException(
                        "Los desarrolladores y administradores no pueden acceder por la WebApp.");
                break;

            case "api":
                if (roles.Any(r => r == "Client" || r == "Agent"))
                    throw new ForbiddenAccessException(
                        "Los clientes y agentes no pueden autenticarse en la API.");
                break;

            default:
                throw new ForbiddenAccessException("Canal de acceso no válido.");
        }
    }
}
