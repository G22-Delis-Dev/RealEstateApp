using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.Entities;

public class PropertyImage : BaseEntity
{
    public string Url { get; set; } = null!;

    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;
}