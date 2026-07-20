using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Administrador")]
[Route("Admin/SaleTypes")]
public class SaleTypesController : BaseController
{
    private readonly ISaleTypeService _saleTypeService;

    public SaleTypesController(ISaleTypeService saleTypeService)
    {
        _saleTypeService = saleTypeService;
    }

    public async Task<IActionResult> Index()
    {
        var types = await _saleTypeService.GetAllAsync();
        if (!types.Any())
            TempData["InfoMessage"] = "No existen tipos de ventas registrados.";
        return View(types);
    }

    public IActionResult Create() => View(new SaleTypeViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(SaleTypeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _saleTypeService.CreateAsync(model);
            return RedirectWithSuccess(nameof(Index), "El tipo de venta fue creado correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _saleTypeService.GetByIdAsync(id);
        if (model is null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SaleTypeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _saleTypeService.UpdateAsync(id, model);
            return RedirectWithSuccess(nameof(Index), "El tipo de venta fue actualizado correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var model = await _saleTypeService.GetByIdAsync(id);
        if (model is null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _saleTypeService.DeleteAsync(id);
        return RedirectWithSuccess(nameof(Index), "El tipo de venta fue eliminado correctamente.");
    }
}