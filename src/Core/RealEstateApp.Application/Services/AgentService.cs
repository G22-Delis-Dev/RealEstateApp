using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Agents;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class AgentService : IAgentService
{
    private readonly IAgentQueryRepository _agentQueryRepository;
    private readonly IPropertyAdminRepository _propertyAdminRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AgentService(
        IAgentQueryRepository agentQueryRepository,
        IPropertyAdminRepository propertyAdminRepository,
        IPropertyRepository propertyRepository,
        IAuthService authService,
        IMapper mapper)
    {
        _agentQueryRepository = agentQueryRepository;
        _propertyAdminRepository = propertyAdminRepository;
        _propertyRepository = propertyRepository;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AgentListItemViewModel>> GetAllAsync()
    {
        var agents = await _authService.GetUsersByRoleAsync("Agent");

        var result = new List<AgentListItemViewModel>();
        foreach (var agent in agents)
        {
            var count = await _agentQueryRepository.CountPropertiesByAgentAsync(agent.Id);
            result.Add(new AgentListItemViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                Email = agent.Email,
                PhotoUrl = agent.PhotoUrl,
                PropertyCount = count,
                IsActive = agent.IsActive
            });
        }

        return result;
    }

    public async Task<AgentViewModel?> GetByIdAsync(string agentId)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(agentId, "Agent");
        if (agent is null) return null;

        var count = await _agentQueryRepository.CountPropertiesByAgentAsync(agentId);

        return new AgentViewModel
        {
            Id = agent.Id,
            FirstName = agent.FirstName,
            LastName = agent.LastName,
            Email = agent.Email,
            Phone = agent.PhoneNumber,
            PhotoUrl = agent.PhotoUrl,
            PropertyCount = count,
            IsActive = agent.IsActive
        };
    }

    public async Task<IEnumerable<PropertyViewModel>> GetAgentPropertiesAsync(string agentId)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(agentId, "Agent")
            ?? throw new NotFoundException("Agente", agentId);

        var properties = await _propertyRepository.GetByAgentIdAsync(agentId);
        return _mapper.Map<IEnumerable<PropertyViewModel>>(properties);
    }

    public async Task<IEnumerable<PropertyViewModel>> GetAgentPropertiesIncludingSoldAsync(string agentId)
    {
        var properties = await _propertyAdminRepository.GetByAgentIdIncludingSoldAsync(agentId);
        return _mapper.Map<IEnumerable<PropertyViewModel>>(properties);
    }

    public async Task<IEnumerable<PropertyDto>> GetAgentPropertiesForApiAsync(string agentId)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(agentId, "Agent")
            ?? throw new NotFoundException("Agente", agentId);

        // API retorna TODAS las propiedades del agente (disponibles y vendidas)
        var properties = await _propertyAdminRepository.GetByAgentIdIncludingSoldAsync(agentId);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task ChangeStatusAsync(string agentId, bool isActive)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(agentId, "Agent")
            ?? throw new NotFoundException("Agente", agentId);

        await _authService.SetUserStatusAsync(agentId, isActive);
    }

    public async Task DeleteAgentAsync(string agentId)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(agentId, "Agent")
            ?? throw new NotFoundException("Agente", agentId);

        // Eliminar todas las propiedades del agente con sus datos relacionados
        var properties = await _propertyAdminRepository.GetByAgentIdIncludingSoldAsync(agentId);
        foreach (var property in properties)
            await _propertyAdminRepository.DeleteWithRelatedDataAsync(property.Id);

        await _authService.DeleteUserAsync(agentId);
    }
}