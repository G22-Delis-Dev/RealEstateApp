using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
