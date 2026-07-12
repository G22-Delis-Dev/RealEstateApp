using RealEstateApp.Application.ViewModels.Properties;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IPropertyService : IGenericService<PropertyViewModel>
{
    Task<PropertyViewModel?> GetByCodeAsync(string code);
    Task<IEnumerable<PropertyViewModel>> GetAvailableAsync();
    Task<IEnumerable<PropertyViewModel>> FilterAsync(PropertyFilterViewModel filter);
    Task<IEnumerable<PropertyViewModel>> GetByAgentIdAsync(string agentId);

    Task<PropertyViewModel> CreateAsync(CreatePropertyViewModel model, string agentId);
    Task UpdateAsync(int id, EditPropertyViewModel model, string agentId);
    Task DeleteAsync(int id, string agentId);
}