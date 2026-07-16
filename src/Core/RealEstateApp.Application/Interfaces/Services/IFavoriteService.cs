using RealEstateApp.Application.ViewModels.Properties;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<IEnumerable<PropertyViewModel>> GetByClientAsync(string clientId);
    Task ToggleAsync(string clientId, int propertyId);
}