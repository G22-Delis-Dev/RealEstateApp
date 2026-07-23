using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Account;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Administrators")]
public class AdministratorsController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IAccountService _accountService;

    public AdministratorsController(IAuthService authService, IAccountService accountService)
    {
        _authService = authService;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var admins = await _authService.GetUsersByRoleAsync("Admin");
        return View(admins);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new CreateAdministratorViewModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateAdministratorViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new RegisterAdministratorRequestDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                PhoneNumber = model.Phone,
                IdCard = model.IdCard
            };

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _accountService.CreateAdministratorAsync(request, currentUserId);
            
            return RedirectWithSuccess(nameof(Index), "Administrador creado correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost("ToggleStatus")]
    public async Task<IActionResult> ToggleStatus(string id, bool isActive)
    {
        try
        {
            if (isActive)
            {
                await _authService.SetUserStatusAsync(id, true);
                return RedirectWithSuccess(nameof(Index), "Administrador activado correctamente.");
            }
            else
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _accountService.DeactivateAdministratorAsync(id, currentUserId);
                return RedirectWithSuccess(nameof(Index), "Administrador inactivado correctamente.");
            }
        }
        catch (BusinessRuleValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
