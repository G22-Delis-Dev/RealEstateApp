using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Administrador")]
[Route("Admin/PropertyTypes")]
public class PropertyTypesController : BaseController
{
    private readonly IPropertyTypeService _propertyTypeService;

    public PropertyTypesController(IPropertyTypeService propertyTypeService)
    {
        _propertyTypeService = propertyTypeService;
    }

    public async Task<IActionResult> Index()
    {
        var types = await _propertyTypeService.GetAllAsync();
        if (!types.Any())
            TempData["InfoMessage"] = "No existen tipos de propiedades registrados.";
        return View(types);
    }

    public IActionResult Create() => View(new PropertyTypeViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(PropertyTypeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _propertyTypeService.CreateAsync(model);
            return RedirectWithSuccess(nameof(Index), "El tipo de propiedad fue creado correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _propertyTypeService.GetByIdAsync(id);
        if (model is null)
        {
            TempData["ErrorMessage"] = "El tipo de propiedad seleccionado no existe.";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PropertyTypeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _propertyTypeService.UpdateAsync(id, model);
            return RedirectWithSuccess(nameof(Index), "El tipo de propiedad fue actualizado correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var model = await _propertyTypeService.GetByIdAsync(id);
        if (model is null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _propertyTypeService.DeleteAsync(id);
        return RedirectWithSuccess(nameof(Index), "El tipo de propiedad fue eliminado correctamente.");
    }
}