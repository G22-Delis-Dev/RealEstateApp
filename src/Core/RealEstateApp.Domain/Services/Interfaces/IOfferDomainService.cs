namespace RealEstateApp.Domain.Services.Interfaces;

public interface IOfferDomainService
{
    Task AcceptOfferAsync(int offerId);
    Task RejectOfferAsync(int offerId);
}