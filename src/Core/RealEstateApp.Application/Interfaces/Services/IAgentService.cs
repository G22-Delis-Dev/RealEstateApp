using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.ViewModels.Agents;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IAgentService
{
    Task<IEnumerable<AgentListItemViewModel>> GetAllAsync();
    Task<AgentViewModel?> GetByIdAsync(string agentId);
    Task<IEnumerable<ViewModels.Properties.PropertyViewModel>> GetAgentPropertiesAsync(string agentId);
    Task ChangeStatusAsync(string agentId, bool isActive);

    // Exclusivo para la API — retorna PropertyDto, nunca el ViewModel de la WebApp (regla Rev. 03)
    Task<IEnumerable<PropertyDto>> GetAgentPropertiesForApiAsync(string agentId);
}