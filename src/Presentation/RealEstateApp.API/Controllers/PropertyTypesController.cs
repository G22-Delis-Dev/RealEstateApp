using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.API.Controllers;

public class PropertyTypesController : BaseApiController
{
    private readonly IPropertyTypeService _propertyTypeService;

    public PropertyTypesController(IPropertyTypeService propertyTypeService)
    {
        _propertyTypeService = propertyTypeService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<IEnumerable<PropertyTypeDto>>> List()
    {
        var types = await _propertyTypeService.GetAllAsync();
        if (!types.Any()) return NoContent();

        return Ok(types.Select(t => new PropertyTypeDto { Id = t.Id, Name = t.Name, Description = t.Description }));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<PropertyTypeDto>> GetById(int id)
    {
        var type = await _propertyTypeService.GetByIdAsync(id);
        if (type is null) return NotFound(new { message = "El tipo de propiedad solicitado no existe." });

        return Ok(new PropertyTypeDto { Id = type.Id, Name = type.Name, Description = type.Description });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PropertyTypeDto>> Create([FromBody] PropertyTypeViewModel request)
    {
        try
        {
            var created = await _propertyTypeService.CreateAsync(request);
            var dto = new PropertyTypeDto { Id = created.Id, Name = created.Name, Description = created.Description };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] PropertyTypeViewModel request)
    {
        try
        {
            await _propertyTypeService.UpdateAsync(id, request);
            return Ok(new { message = "El tipo de propiedad fue actualizado correctamente." });
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var type = await _propertyTypeService.GetByIdAsync(id);
        if (type is null) return NotFound(new { message = "El tipo de propiedad solicitado no existe." });

        await _propertyTypeService.DeleteAsync(id);
        return NoContent();
    }
}