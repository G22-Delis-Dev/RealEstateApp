using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.Catalogs;

public class SaleTypeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del tipo de venta es requerido.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida.")]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public int PropertyCount { get; set; }
}