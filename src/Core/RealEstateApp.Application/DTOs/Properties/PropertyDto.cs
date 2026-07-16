namespace RealEstateApp.Application.DTOs.Properties;

public class PropertyDto
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
    public List<string> Improvements { get; set; } = new();
    public string AgentName { get; set; } = null!;
    public string AgentId { get; set; } = null!;
    public string Status { get; set; } = null!; // "Disponible" | "Vendida"
}