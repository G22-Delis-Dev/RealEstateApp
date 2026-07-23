using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Debe ingresar su correo o nombre de usuario.")]
    [Display(Name = "Correo o nombre de usuario")]
    public string UserOrEmail { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
