using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Offers;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Domain.Services.Interfaces;

namespace RealEstateApp.Application.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOfferFactory _offerFactory;
    private readonly IOfferDomainService _offerDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OfferService(
        IOfferRepository offerRepository,
        IPropertyRepository propertyRepository,
        IOfferFactory offerFactory,
        IOfferDomainService offerDomainService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _offerRepository = offerRepository;
        _propertyRepository = propertyRepository;
        _offerFactory = offerFactory;
        _offerDomainService = offerDomainService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferViewModel>> GetByClientAndPropertyAsync(string clientId, int propertyId)
    {
        var offers = await _offerRepository.GetByClientAndPropertyAsync(clientId, propertyId);
        return _mapper.Map<IEnumerable<OfferViewModel>>(offers);
    }

    public async Task<IEnumerable<OfferViewModel>> GetByPropertyAsync(int propertyId)
    {
        var offers = await _offerRepository.GetByPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<OfferViewModel>>(offers);
    }

    public async Task<OfferViewModel> CreateAsync(CreateOfferViewModel model, string clientId)
    {
        var property = await _propertyRepository.GetByIdAsync(model.PropertyId)
            ?? throw new NotFoundException(nameof(Domain.Entities.Property), model.PropertyId);

        var clientOffers = await _offerRepository.GetByClientAndPropertyAsync(clientId, model.PropertyId);

        var offer = _offerFactory.Create(property, clientId, model.Amount, clientOffers);

        await _offerRepository.AddAsync(offer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OfferViewModel>(offer);
    }

    public async Task AcceptAsync(int offerId, string agentId)
    {
        await EnsureAgentOwnsOfferPropertyAsync(offerId, agentId);
        await _offerDomainService.AcceptOfferAsync(offerId);
    }

    public async Task RejectAsync(int offerId, string agentId)
    {
        await EnsureAgentOwnsOfferPropertyAsync(offerId, agentId);
        await _offerDomainService.RejectOfferAsync(offerId);
    }

    private async Task EnsureAgentOwnsOfferPropertyAsync(int offerId, string agentId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId)
            ?? throw new NotFoundException(nameof(Domain.Entities.Offer), offerId);

        var property = await _propertyRepository.GetByIdAsync(offer.PropertyId)
            ?? throw new NotFoundException(nameof(Domain.Entities.Property), offer.PropertyId);

        if (property.AgentId != agentId)
            throw new ForbiddenAccessException("No tiene permisos para gestionar ofertas de esta propiedad.");
    }
}