namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string PropertyTypeName { get; set; } = null!;
    public string SaleTypeName { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public List<string> ImageUrls { get; set; } = new();
    public List<string> Improvements { get; set; } = new();
    public string AgentId { get; set; } = null!;
}