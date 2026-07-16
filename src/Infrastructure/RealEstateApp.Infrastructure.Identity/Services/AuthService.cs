using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new Exception($"No se encontró una cuenta con el correo {request.Email}.");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new Exception("Credenciales incorrectas.");

        if (!user.IsActive)
            throw new Exception("Tu cuenta no está activa. Contacta al administrador.");

        if (!user.EmailConfirmed)
            throw new Exception("Tu cuenta no ha sido confirmada. Revisa tu correo electrónico.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);

        return new LoginResponseDto
        {
            Token = token,
            UserName = user.UserName!,
            Roles = roles.ToList(),
            Expiration = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<string> RegisterDeveloperAsync(RegisterDeveloperRequestDto request, string origin)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "Developer");

        // Generar token de confirmación y enviar correo de activación
        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationToken));
        var activationLink = $"{origin}/api/Account/confirm-email?userId={user.Id}&token={encodedToken}";

        await _emailService.SendAccountActivationEmailAsync(user.Email!, activationLink);

        return $"Usuario {request.UserName} registrado exitosamente. Revisa tu correo para activar tu cuenta.";
    }

    public async Task<string> RegisterAdministratorAsync(RegisterAdministratorRequestDto request, string origin)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IdCard = request.IdCard,
            IsActive = true // Los administradores se activan de inmediato
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "Admin");

        return $"Administrador {request.UserName} registrado exitosamente.";
    }

    public async Task<string> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new Exception("No se encontró el usuario.");

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
            throw new Exception("Error al confirmar el correo electrónico.");

        // Activar la cuenta después de confirmar el correo
        user.IsActive = true;
        await _userManager.UpdateAsync(user);

        return "Correo electrónico confirmado exitosamente. Tu cuenta está activa.";
    }
}
