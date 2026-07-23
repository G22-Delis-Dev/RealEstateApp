using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
