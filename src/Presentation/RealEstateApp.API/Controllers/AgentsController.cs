using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Agents;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.API.Controllers;

public class AgentsController : BaseApiController
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    // GET: api/agents
    [HttpGet]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<IEnumerable<AgentDto>>> List()
    {
        var agents = await _agentService.GetAllAsync();
        if (!agents.Any())
            return NoContent();

        var dtos = agents.Select(a => new AgentDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            PropertyCount = a.PropertyCount,
            Email = a.Email,
            IsActive = a.IsActive
        });

        return Ok(dtos);
    }

    // GET: api/agents/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<ActionResult<AgentDto>> GetById(string id)
    {
        var agent = await _agentService.GetByIdAsync(id);
        if (agent is null)
            return NotFound(new { message = "El agente solicitado no existe." });

        var dto = new AgentDto
        {
            Id = agent.Id,
            FirstName = agent.FirstName,
            LastName = agent.LastName,
            PropertyCount = agent.PropertyCount,
            Email = agent.Email,
            Phone = agent.Phone,
            IsActive = agent.IsActive
        };

        return Ok(dto);
    }

    // GET: api/agents/{id}/properties
    [HttpGet("{id}/properties")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public async Task<IActionResult> GetAgentProperty(string id)
    {
        var agent = await _agentService.GetByIdAsync(id);
        if (agent is null)
            return NotFound(new { message = "El agente solicitado no existe." });

        var properties = await _agentService.GetAgentPropertiesAsync(id);
        if (!properties.Any())
            return NoContent();

        return Ok(properties);
    }

    // PATCH: api/agents/{id}/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeAgentStatusRequest request)
    {
        var agent = await _agentService.GetByIdAsync(id);
        if (agent is null)
            return NotFound(new { message = "El agente solicitado no existe." });

        await _agentService.ChangeStatusAsync(id, request.IsActive);
        return NoContent();
    }
}

public class ChangeAgentStatusRequest
{
    public bool IsActive { get; set; }
}