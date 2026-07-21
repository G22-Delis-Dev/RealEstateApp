using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.WebApp.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/AdminHome")]
public class AdminHomeController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IPropertyService _propertyService;

    public AdminHomeController(IAuthService authService, IPropertyService propertyService)
    {
        _authService = authService;
        _propertyService = propertyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Get all users by role
        var agents = await _authService.GetUsersByRoleAsync("Agent");
        var clients = await _authService.GetUsersByRoleAsync("Client");
        var developers = await _authService.GetUsersByRoleAsync("Developer");

        // Property count
        // Note: For a production app with many properties, a dedicated CountAsync method would be better.
        var properties = await _propertyService.GetAllAsync();
        var totalProperties = properties.Count();

        ViewBag.TotalProperties = totalProperties;
        
        ViewBag.ActiveAgents = agents.Count(a => a.IsActive);
        ViewBag.InactiveAgents = agents.Count(a => !a.IsActive);
        
        ViewBag.ActiveClients = clients.Count(c => c.IsActive);
        ViewBag.InactiveClients = clients.Count(c => !c.IsActive);
        
        ViewBag.ActiveDevelopers = developers.Count(d => d.IsActive);
        ViewBag.InactiveDevelopers = developers.Count(d => !d.IsActive);

        return View();
    }
}
