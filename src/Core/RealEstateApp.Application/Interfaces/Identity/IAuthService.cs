using System.Threading.Tasks;

namespace RealEstateApp.Application.Interfaces.Identity;

public interface IAuthService
{
    // Las firmas exactas dependerán de los DTOs que se creen posteriormente
    // Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    // Task<string> RegisterAsync(RegisterRequest request, string origin);
    // Task<string> ConfirmEmailAsync(string userId, string code);
}
