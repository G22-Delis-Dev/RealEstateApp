namespace RealEstateApp.Application.DTOs.Account;

public class LoginRequestDto
{
    /// <summary>Correo electrónico o nombre de usuario.</summary>
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
