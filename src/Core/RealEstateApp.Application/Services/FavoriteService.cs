using AutoMapper;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IFavoriteFactory _favoriteFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FavoriteService(
        IFavoriteRepository favoriteRepository,
        IFavoriteFactory favoriteFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _favoriteRepository = favoriteRepository;
        _favoriteFactory = favoriteFactory;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyViewModel>> GetByClientAsync(string clientId)
    {
        var favorites = await _favoriteRepository.GetByClientAsync(clientId);
        return _mapper.Map<IEnumerable<PropertyViewModel>>(favorites.Select(f => f.Property));
    }

    public async Task ToggleAsync(string clientId, int propertyId)
    {
        var alreadyFavorited = await _favoriteRepository.ExistsAsync(clientId, propertyId);

        if (alreadyFavorited)
        {
            await _favoriteRepository.RemoveAsync(clientId, propertyId);
        }
        else
        {
            var favorite = _favoriteFactory.Create(clientId, propertyId, alreadyFavorited: false);
            await _favoriteRepository.AddAsync(favorite);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}