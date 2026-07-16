using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Administrador")]
[Route("Admin/Improvements")]
public class ImprovementsController : BaseController
{
    private readonly IImprovementService _improvementService;

    public ImprovementsController(IImprovementService improvementService)
    {
        _improvementService = improvementService;
    }

    public async Task<IActionResult> Index()
    {
        var improvements = await _improvementService.GetAllAsync();
        if (!improvements.Any())
            TempData["InfoMessage"] = "No existen mejoras registradas.";
        return View(improvements);
    }

    public IActionResult Create() => View(new ImprovementViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(ImprovementViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _improvementService.CreateAsync(model);
            return RedirectWithSuccess(nameof(Index), "La mejora fue creada correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _improvementService.GetByIdAsync(id);
        if (model is null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ImprovementViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _improvementService.UpdateAsync(id, model);
            return RedirectWithSuccess(nameof(Index), "La mejora fue actualizada correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var model = await _improvementService.GetByIdAsync(id);
        if (model is null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _improvementService.DeleteAsync(id);
        return RedirectWithSuccess(nameof(Index), "La mejora fue eliminada correctamente.");
    }
}