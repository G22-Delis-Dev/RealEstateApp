using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.Account;

public class EditDeveloperViewModel
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es requerido.")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es requerido.")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [DataType(DataType.Text)]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es requerido.")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "La cédula es requerida.")]
    [DataType(DataType.Text)]
    public string IdCard { get; set; } = null!;

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string? ConfirmNewPassword { get; set; }
}
