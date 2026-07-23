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

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var agent = await _agentService.GetByIdAsync(id);
        if (agent is null)
        {
            TempData["ErrorMessage"] = "El agente solicitado no existe.";
            return RedirectToAction(nameof(Index));
        }
        return View(agent);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            await _agentService.DeleteAgentAsync(id);
            return RedirectWithSuccess(nameof(Index), "El agente y sus propiedades asociadas fueron eliminados correctamente.");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}