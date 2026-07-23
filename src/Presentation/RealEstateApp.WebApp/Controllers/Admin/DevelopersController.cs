using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Account;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Developers")]
public class DevelopersController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IAccountService _accountService;

    public DevelopersController(IAuthService authService, IAccountService accountService)
    {
        _authService = authService;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var developers = await _authService.GetUsersByRoleAsync("Developer");
        return View(developers);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new CreateDeveloperViewModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateDeveloperViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new RegisterDeveloperRequestDto
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

            var origin = Request.Headers["origin"].ToString();
            if (string.IsNullOrWhiteSpace(origin))
            {
                origin = $"{Request.Scheme}://{Request.Host}";
            }

            await _accountService.CreateDeveloperAsync(request, origin);
            
            return RedirectWithSuccess(nameof(Index), "Desarrollador creado correctamente. Se ha enviado un correo para su activación.");
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
            await _authService.SetUserStatusAsync(id, isActive);
            var statusStr = isActive ? "activado" : "inactivado";
            return RedirectWithSuccess(nameof(Index), $"Desarrollador {statusStr} correctamente.");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var dev = await _authService.GetUserByIdInRoleAsync(id, "Developer");
            if (dev is null)
            {
                TempData["ErrorMessage"] = "Desarrollador no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditDeveloperViewModel
            {
                Id = dev.Id,
                FirstName = dev.FirstName,
                LastName = dev.LastName,
                UserName = dev.UserName,
                Email = dev.Email,
                Phone = dev.PhoneNumber ?? "",
                IdCard = dev.IdCard ?? ""
            };

            return View(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit(EditDeveloperViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _authService.UpdateDeveloperOrAdminAsync(
                model.Id,
                model.UserName,
                model.Email,
                model.FirstName,
                model.LastName,
                model.Phone,
                model.IdCard,
                model.NewPassword
            );

            return RedirectWithSuccess(nameof(Index), "Desarrollador actualizado correctamente.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}
