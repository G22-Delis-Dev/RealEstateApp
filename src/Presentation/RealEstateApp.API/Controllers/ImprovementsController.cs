using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.API.Controllers;

public class ImprovementsController : BaseApiController
{
    private readonly IImprovementService _improvementService;

    public ImprovementsController(IImprovementService improvementService)
    {
        _improvementService = improvementService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<IEnumerable<ImprovementDto>>> List()
    {
        var improvements = await _improvementService.GetAllAsync();
        if (!improvements.Any()) return NoContent();

        return Ok(improvements.Select(i => new ImprovementDto { Id = i.Id, Name = i.Name, Description = i.Description }));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<ImprovementDto>> GetById(int id)
    {
        var improvement = await _improvementService.GetByIdAsync(id);
        if (improvement is null) return NotFound(new { message = "La mejora solicitada no existe." });

        return Ok(new ImprovementDto { Id = improvement.Id, Name = improvement.Name, Description = improvement.Description });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ImprovementDto>> Create([FromBody] ImprovementRequestDto request)
    {
        try
        {
            var created = await _improvementService.CreateForApiAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] ImprovementRequestDto request)
    {
        try
        {
            await _improvementService.UpdateForApiAsync(id, request);
            return Ok(new { message = "La mejora fue actualizada correctamente." });
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
        var improvement = await _improvementService.GetByIdAsync(id);
        if (improvement is null) return NotFound(new { message = "La mejora solicitada no existe." });

        await _improvementService.DeleteAsync(id);
        return NoContent();
    }
}