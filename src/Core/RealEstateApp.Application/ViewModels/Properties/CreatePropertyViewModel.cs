using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.ViewModels.Properties;

public class CreatePropertyViewModel
{
    [Required(ErrorMessage = "El tipo de propiedad es requerido.")]
    public int PropertyTypeId { get; set; }

    [Required(ErrorMessage = "El tipo de venta es requerido.")]
    public int SaleTypeId { get; set; }

    [Required(ErrorMessage = "El precio es requerido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "La descripción es requerida.")]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tamaño es requerido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El tamaño debe ser mayor que cero.")]
    public decimal Size { get; set; }

    [Required(ErrorMessage = "La cantidad de habitaciones es requerida.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de habitaciones no puede ser menor que cero.")]
    public int Rooms { get; set; }

    [Required(ErrorMessage = "La cantidad de baños es requerida.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de baños no puede ser menor que cero.")]
    public int Bathrooms { get; set; }

    [Required(ErrorMessage = "Debe seleccionar al menos una mejora.")]
    public List<int> ImprovementIds { get; set; } = new();

    [Required(ErrorMessage = "Debe cargar al menos una imagen.")]
    public List<IFormFile> Images { get; set; } = new();

    // Para poblar los <select> del formulario
    public List<SelectOption> AvailablePropertyTypes { get; set; } = new();
    public List<SelectOption> AvailableSaleTypes { get; set; } = new();
    public List<SelectOption> AvailableImprovements { get; set; } = new();
}

public class SelectOption
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}