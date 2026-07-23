using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.ViewModels.Account;

public class AgentProfileViewModel
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es requerido.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es requerido.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es requerido.")]
    public string Phone { get; set; } = null!;

    /// <summary>URL actual de la foto (para mostrarla en la vista).</summary>
    public string? CurrentPhotoUrl { get; set; }

    /// <summary>Nueva foto opcional. Si es null se conserva la actual.</summary>
    [DataType(DataType.Upload)]
    public IFormFile? NewPhoto { get; set; }
}
