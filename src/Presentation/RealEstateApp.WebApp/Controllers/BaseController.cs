using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.WebApp.Controllers;

public abstract class BaseController : Controller
{
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    protected IActionResult RedirectWithSuccess(string action, string message, string? controller = null)
    {
        TempData["SuccessMessage"] = message;
        return controller is null ? RedirectToAction(action) : RedirectToAction(action, controller);
    }

    protected IActionResult RedirectWithError(string action, string message, string? controller = null)
    {
        TempData["ErrorMessage"] = message;
        return controller is null ? RedirectToAction(action) : RedirectToAction(action, controller);
    }
}