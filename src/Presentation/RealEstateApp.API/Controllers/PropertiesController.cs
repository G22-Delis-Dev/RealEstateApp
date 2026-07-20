using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.API.Controllers;

[Authorize(Roles = "Administrador,Desarrollador")]
public class PropertiesController : BaseApiController
{
    private readonly IPropertyService _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    // GET: api/properties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> List()
    {
        var properties = await _propertyService.GetAllForApiAsync();
        if (!properties.Any())
            return NoContent();

        return Ok(properties);
    }

    // GET: api/properties/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropertyDto>> GetById(int id)
    {
        var property = await _propertyService.GetByIdForApiAsync(id);
        if (property is null)
            return NotFound(new { message = "La propiedad solicitada no existe." });

        return Ok(property);
    }

    // GET: api/properties/code/482913
    [HttpGet("code/{code}")]
    public async Task<ActionResult<PropertyDto>> GetByCode(string code)
    {
        var property = await _propertyService.GetByCodeForApiAsync(code);
        if (property is null)
            return NotFound(new { message = "No existe una propiedad registrada con el código enviado." });

        return Ok(property);
    }
}