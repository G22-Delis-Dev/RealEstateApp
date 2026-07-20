using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs.Account;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;

namespace RealEstateApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IAuthService _authService;

    public AccountController(IAccountService accountService, IAuthService authService)
    {
        _accountService = accountService;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, [FromQuery] string channel = "API")
    {
        // channel = "API" (Postman/desarrolladores) o "WebApp" (MVC App)
        var response = await _accountService.LoginAsync(request, channel);
        return Ok(response);
    }

    [HttpPost("register-client")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterDeveloperRequestDto request)
    {
        var origin = Request.Headers["origin"].ToString();
        var response = await _accountService.RegisterClientAsync(request, origin);
        return Ok(new { message = response });
    }

    [HttpPost("register-agent")]
    public async Task<IActionResult> RegisterAgent([FromBody] RegisterDeveloperRequestDto request)
    {
        var response = await _accountService.RegisterAgentAsync(request);
        return Ok(new { message = response });
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdministratorRequestDto request)
    {
        var origin = Request.Headers["origin"].ToString();
        var response = await _authService.RegisterAdministratorAsync(request, origin);
        return Ok(new { message = response });
    }

    [HttpPost("register-developer")]
    public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterDeveloperRequestDto request)
    {
        var origin = Request.Headers["origin"].ToString();
        var response = await _authService.RegisterDeveloperAsync(request, origin);
        return Ok(new { message = response });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var response = await _accountService.ConfirmEmailAsync(userId, token);
        return Ok(new { message = response });
    }
}
