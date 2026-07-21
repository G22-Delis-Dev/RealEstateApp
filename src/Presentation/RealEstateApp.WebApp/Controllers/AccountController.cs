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
            return RedirectToAction("Index", "Home");

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
                Email = model.Email,
                Password = model.Password
            };

            await _accountService.LoginAsync(request, "WebApp");
            
            // Check role to redirect appropriately
            if (User.IsInRole("Admin")) return RedirectToAction("Index", "AdminHome");
            if (User.IsInRole("Developer") || User.IsInRole("Client")) return RedirectToAction("Index", "Home");

            // For agents or others, redirect to home
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
            return RedirectToAction("Index", "Home");

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
                TempData["SuccessMessage"] = "Tu cuenta de Agente fue creada con éxito. Un administrador deberá activarla.";
            }
            else // Client (o Developer)
            {
                await _accountService.RegisterClientAsync(request, origin);
                TempData["SuccessMessage"] = "Tu cuenta fue creada con éxito. Verifica tu correo para activarla.";
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
}
