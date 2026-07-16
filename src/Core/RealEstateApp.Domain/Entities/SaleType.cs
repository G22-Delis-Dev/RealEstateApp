using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.Entities;

public class SaleType : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Property> Properties { get; set; } = new List<Property>();
}