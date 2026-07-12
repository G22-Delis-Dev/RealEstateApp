using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyFilterViewModel
{
    public int? PropertyTypeId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo no puede ser menor que cero.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio máximo no puede ser menor que cero.")]
    public decimal? MaxPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de habitaciones no puede ser menor que cero.")]
    public int? Rooms { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de baños no puede ser menor que cero.")]
    public int? Bathrooms { get; set; }

    public string? Code { get; set; }

    public List<SelectOption> AvailablePropertyTypes { get; set; } = new();
}