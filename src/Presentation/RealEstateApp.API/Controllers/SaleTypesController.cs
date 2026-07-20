using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.API.Controllers;

public class SaleTypesController : BaseApiController
{
    private readonly ISaleTypeService _saleTypeService;

    public SaleTypesController(ISaleTypeService saleTypeService)
    {
        _saleTypeService = saleTypeService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<IEnumerable<SaleTypeDto>>> List()
    {
        var types = await _saleTypeService.GetAllAsync();
        if (!types.Any()) return NoContent();

        return Ok(types.Select(t => new SaleTypeDto { Id = t.Id, Name = t.Name, Description = t.Description }));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<SaleTypeDto>> GetById(int id)
    {
        var type = await _saleTypeService.GetByIdAsync(id);
        if (type is null) return NotFound(new { message = "El tipo de venta solicitado no existe." });

        return Ok(new SaleTypeDto { Id = type.Id, Name = type.Name, Description = type.Description });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<SaleTypeDto>> Create([FromBody] SaleTypeRequestDto request)
    {
        try
        {
            var created = await _saleTypeService.CreateForApiAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] SaleTypeRequestDto request)
    {
        try
        {
            await _saleTypeService.UpdateForApiAsync(id, request);
            return Ok(new { message = "El tipo de venta fue actualizado correctamente." });
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var type = await _saleTypeService.GetByIdAsync(id);
        if (type is null) return NotFound(new { message = "El tipo de venta solicitado no existe." });

        await _saleTypeService.DeleteAsync(id);
        return NoContent();
    }


}