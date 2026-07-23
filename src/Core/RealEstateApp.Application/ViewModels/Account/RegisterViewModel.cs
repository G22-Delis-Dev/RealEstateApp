using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.ViewModels.Account;

public class RegisterViewModel
{
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

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirmar contraseña es requerido.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es requerido.")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de usuario es requerido.")]
    public string UserType { get; set; } = null!;

    [DataType(DataType.Upload)]
    public IFormFile? ProfilePhoto { get; set; }
}
