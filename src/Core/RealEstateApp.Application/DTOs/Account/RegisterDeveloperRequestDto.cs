namespace RealEstateApp.Application.DTOs.Account;

// Registro de desarrollador (cliente): no lleva cédula
public class RegisterDeveloperRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? IdCard { get; set; }
}
