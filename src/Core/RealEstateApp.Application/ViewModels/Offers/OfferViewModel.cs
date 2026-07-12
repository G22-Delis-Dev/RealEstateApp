namespace RealEstateApp.Application.ViewModels.Offers;

public class OfferViewModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = null!; 
    public DateTime OfferedAt { get; set; }
    public int PropertyId { get; set; }
    public string ClientId { get; set; } = null!;
}