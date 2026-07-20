using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;

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
