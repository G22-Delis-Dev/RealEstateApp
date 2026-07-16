using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Properties;

namespace RealEstateApp.WebApp.Controllers;

public class HomeController : BaseController
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;

    public HomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService)
    {
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
    }

    // GET: /
    public async Task<IActionResult> Index(string? code)
    {
        if (!string.IsNullOrWhiteSpace(code))
        {
            var property = await _propertyService.GetByCodeAsync(code);
            if (property is null)
            {
                TempData["ErrorMessage"] = "No se encontró ninguna propiedad disponible con el código ingresado.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { id = property.Id });
        }

        var properties = await _propertyService.GetAvailableAsync();
        await PopulateFilterOptionsAsync();
        return View(properties);
    }

    // POST: /Home/Filter
    [HttpPost]
    public async Task<IActionResult> Filter(PropertyFilterViewModel filter)
    {
        var properties = await _propertyService.FilterAsync(filter);

        if (!properties.Any())
            TempData["InfoMessage"] = "No se encontraron propiedades disponibles con los filtros seleccionados.";

        await PopulateFilterOptionsAsync();
        return View(nameof(Index), properties);
    }

    // GET: /Home/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.Status != "Disponible")
        {
            TempData["ErrorMessage"] = "La propiedad solicitada no existe o no se encuentra disponible.";
            return RedirectToAction(nameof(Index));
        }

        return View(property);
    }

    private async Task PopulateFilterOptionsAsync()
    {
        var types = await _propertyTypeService.GetAllAsync();
        ViewBag.PropertyTypes = types.Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
    }
}