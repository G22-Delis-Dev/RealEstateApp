using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Application.ViewModels.Account;

namespace RealEstateApp.WebApp.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;
    private readonly IFileStorageService _fileStorageService;

    public AccountController(IAccountService accountService, IFileStorageService fileStorageService)
    {
        _accountService = accountService;
        _fileStorageService = fileStorageService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectBasedOnRole();

        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new LoginRequestDto
            {
                Email = model.UserOrEmail,
                Password = model.Password
            };

            // LoginAsync autentica con cookie y retorna los roles del usuario
            var response = await _accountService.LoginAsync(request, "WebApp");

            // Redirigir según el rol usando el response (la cookie ya está activa)
            if (response.Roles.Contains("Admin"))
                return RedirectToAction("Index", "AdminHome", new { area = "" });

            if (response.Roles.Contains("Agent"))
                return RedirectToAction("Index", "AgentProperties", new { area = "" });

            if (response.Roles.Contains("Client"))
                return RedirectToAction("Index", "Client", new { area = "" });

            // Fallback al home público
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectBasedOnRole();

        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            string? profilePicUrl = null;
            if (model.ProfilePhoto != null)
            {
                using var stream = model.ProfilePhoto.OpenReadStream();
                var fileName = $"{Guid.NewGuid()}_{model.ProfilePhoto.FileName}";
                profilePicUrl = await _fileStorageService.UploadFileAsync(stream, fileName, "Users");
            }

            var request = new RegisterDeveloperRequestDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                PhoneNumber = model.Phone,
                ProfilePicture = profilePicUrl
            };

            var origin = Request.Headers["origin"].ToString();
            if (string.IsNullOrWhiteSpace(origin))
            {
                origin = $"{Request.Scheme}://{Request.Host}";
            }

            if (model.UserType == "Agent")
            {
                await _accountService.RegisterAgentAsync(request);
                TempData["SuccessMessage"] = "Su cuenta de agente ha sido creada correctamente. Un administrador debe activar su usuario antes de que pueda iniciar sesión.";
            }
            else // Client
            {
                await _accountService.RegisterClientAsync(request, origin);
                TempData["SuccessMessage"] = "Su cuenta ha sido creada correctamente. Revise su correo electrónico para activar su usuario.";
            }

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return RedirectToAction("Index", "Home");

        try
        {
            await _accountService.ConfirmEmailAsync(userId, token);
            TempData["SuccessMessage"] = "Tu cuenta ha sido activada con éxito. Ya puedes iniciar sesión.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    // ─────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────

    /// <summary>Redirige al home del rol del usuario ya autenticado.</summary>
    private IActionResult RedirectBasedOnRole()
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Index", "AdminHome");
        if (User.IsInRole("Agent"))
            return RedirectToAction("Index", "AgentProperties");
        if (User.IsInRole("Client"))
            return RedirectToAction("Index", "Client");
        return RedirectToAction("Index", "Home");
    }
}
