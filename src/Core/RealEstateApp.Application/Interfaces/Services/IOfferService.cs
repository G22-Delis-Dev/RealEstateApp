using RealEstateApp.Application.ViewModels.Offers;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IOfferService
{
    Task<IEnumerable<OfferViewModel>> GetByClientAndPropertyAsync(string clientId, int propertyId);
    Task<IEnumerable<OfferViewModel>> GetByPropertyAsync(int propertyId);
    Task<OfferViewModel> CreateAsync(CreateOfferViewModel model, string clientId);
    Task AcceptAsync(int offerId, string agentId);
    Task RejectAsync(int offerId, string agentId);
}