using System.Collections.Generic;
using System.Security.Claims;

namespace RealEstateApp.Application.Interfaces.Identity;

public interface IJwtTokenService
{
    // Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
    // Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
}
