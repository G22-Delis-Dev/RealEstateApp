using RealEstateApp.Domain.Enums;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Offer;
using RealEstateApp.Domain.Services.Interfaces;

namespace RealEstateApp.Domain.Services.Implementations;

public class OfferDomainService : IOfferDomainService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OfferDomainService(
        IOfferRepository offerRepository,
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork)
    {
        _offerRepository = offerRepository;
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AcceptOfferAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId)
            ?? throw new InvalidOperationException("La oferta no existe.");

        BusinessRuleValidator.CheckRule(new OfferMustBePendingToRespondRule(offer.Status));

        offer.Status = OfferStatus.Aceptada;

        var property = await _propertyRepository.GetByIdAsync(offer.PropertyId)
            ?? throw new InvalidOperationException("La propiedad no existe.");
        property.Status = PropertyStatus.Vendida;

        var otherPendingOffers = await _offerRepository.GetPendingByPropertyAsync(offer.PropertyId, excludeOfferId: offerId);
        foreach (var other in otherPendingOffers)
            other.Status = OfferStatus.Rechazada;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RejectOfferAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId)
            ?? throw new InvalidOperationException("La oferta no existe.");

        BusinessRuleValidator.CheckRule(new OfferMustBePendingToRespondRule(offer.Status));

        offer.Status = OfferStatus.Rechazada;
        await _unitOfWork.SaveChangesAsync();
    }
}