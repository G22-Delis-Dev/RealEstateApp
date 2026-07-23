using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Agents")]
public class AgentManagementController : BaseController
{
    private readonly IAgentService _agentService;

    public AgentManagementController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    public async Task<IActionResult> Index()
    {
        var agents = await _agentService.GetAllAsync();
        return View(agents);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(string agentId, bool activate)
    {
        await _agentService.ChangeStatusAsync(agentId, isActive: activate);

        var message = activate ? "El agente fue activado correctamente." : "El agente fue inactivado correctamente.";
        return RedirectWithSuccess(nameof(Index), message);
    }
}