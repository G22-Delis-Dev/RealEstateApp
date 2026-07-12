namespace RealEstateApp.Application.DTOs.Account;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
    public DateTime Expiration { get; set; }
}
