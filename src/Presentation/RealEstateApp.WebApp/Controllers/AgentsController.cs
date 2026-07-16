using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.WebApp.Controllers;

public class AgentsController : BaseController
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    // GET: /Agents
    public async Task<IActionResult> Index(string? name)
    {
        var agents = await _agentService.GetAllAsync(); // ya viene filtrado a activos según implementación de Sky en IAuthService
        var activeAgents = agents.Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(name))
        {
            activeAgents = activeAgents.Where(a =>
                a.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                a.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!activeAgents.Any())
            TempData["InfoMessage"] = "No se encontraron agentes activos con el nombre ingresado.";

        return View(activeAgents.OrderBy(a => a.FirstName).ThenBy(a => a.LastName));
    }

    // GET: /Agents/Properties/{agentId}
    public async Task<IActionResult> Properties(string agentId)
    {
        var agent = await _agentService.GetByIdAsync(agentId);
        if (agent is null || !agent.IsActive)
        {
            TempData["ErrorMessage"] = "El agente solicitado no existe o no se encuentra disponible.";
            return RedirectToAction(nameof(Index));
        }

        var properties = await _agentService.GetAgentPropertiesAsync(agentId);
        var availableOnly = properties.Where(p => p.Status == "Disponible");

        if (!availableOnly.Any())
            TempData["InfoMessage"] = "Este agente no tiene propiedades disponibles en este momento.";

        ViewBag.Agent = agent;
        return View(availableOnly);
    }
}