using RealEstateApp.Domain.Common;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Entities;

public class Property : AuditableEntity
{
    public string Code { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public decimal Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public PropertyStatus Status { get; set; }

    public int PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;

    public int SaleTypeId { get; set; }
    public SaleType SaleType { get; set; } = null!;

    public string AgentId { get; set; } = null!;

    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<Improvement> Improvements { get; set; } = new List<Improvement>();
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}