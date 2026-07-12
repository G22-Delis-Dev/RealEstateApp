using RealEstateApp.Domain.Common;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Entities;

public class Offer : AuditableEntity
{
    public decimal Amount { get; set; }
    public OfferStatus Status { get; set; }
    public DateTime OfferedAt { get; set; }

    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public string ClientId { get; set; } = null!;
}