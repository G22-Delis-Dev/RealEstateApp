using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.Interfaces.Identity;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Domain.Services.Interfaces;

namespace RealEstateApp.Application.Services;

public class PropertyService : GenericService<PropertyViewModel, Domain.Entities.Property>, IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IPropertyAdminRepository _propertyAdminRepository;
    private readonly IImprovementRepository _improvementRepository;
    private readonly IPropertyFactory _propertyFactory;
    private readonly IPropertyCodeDomainService _propertyCodeDomainService;
    private readonly IPropertyDomainService _propertyDomainService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IAuthService _authService; // ← nuevo

    public PropertyService(
        IPropertyRepository propertyRepository,
        IPropertyAdminRepository propertyAdminRepository,
        IImprovementRepository improvementRepository,
        IPropertyFactory propertyFactory,
        IPropertyCodeDomainService propertyCodeDomainService,
        IPropertyDomainService propertyDomainService,
        IFileStorageService fileStorageService,
        IAuthService authService, // ← nuevo
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(propertyRepository, unitOfWork, mapper)
    {
        _propertyRepository = propertyRepository;
        _propertyAdminRepository = propertyAdminRepository;
        _improvementRepository = improvementRepository;
        _propertyFactory = propertyFactory;
        _propertyCodeDomainService = propertyCodeDomainService;
        _propertyDomainService = propertyDomainService;
        _fileStorageService = fileStorageService;
        _authService = authService; // ← nuevo
    }

    public override async Task<PropertyViewModel?> GetByIdAsync(int id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property is null) return null;
        var vm = _mapper.Map<PropertyViewModel>(property);
        await PopulateAgentInfoAsync(vm);
        return vm;
    }

    public async Task<PropertyViewModel?> GetByCodeAsync(string code)
    {
        var property = await _propertyRepository.GetByCodeAsync(code);
        if (property is null) return null;
        var vm = _mapper.Map<PropertyViewModel>(property);
        await PopulateAgentInfoAsync(vm);
        return vm;
    }

    public async Task<IEnumerable<PropertyViewModel>> GetAvailableAsync()
    {
        var properties = await _propertyRepository.GetAvailableAsync();
        var vms = _mapper.Map<List<PropertyViewModel>>(properties);
        await PopulateAgentInfoBatchAsync(vms);
        return vms;
    }

    public async Task<IEnumerable<PropertyViewModel>> FilterAsync(PropertyFilterViewModel filter)
    {
        var properties = await _propertyRepository.FilterAsync(
            filter.PropertyTypeId, filter.MinPrice, filter.MaxPrice, filter.Rooms, filter.Bathrooms);
        var vms = _mapper.Map<List<PropertyViewModel>>(properties);
        await PopulateAgentInfoBatchAsync(vms);
        return vms;
    }

    public async Task<IEnumerable<PropertyViewModel>> GetByAgentIdAsync(string agentId)
    {
        var properties = await _propertyRepository.GetByAgentIdAsync(agentId);
        return _mapper.Map<IEnumerable<PropertyViewModel>>(properties);
    }

    public async Task<IEnumerable<PropertyViewModel>> GetByAgentIdIncludingSoldAsync(string agentId)
    {
        var properties = await _propertyAdminRepository.GetByAgentIdIncludingSoldAsync(agentId);
        return _mapper.Map<IEnumerable<PropertyViewModel>>(properties);
    }

    public async Task<PropertyViewModel> CreateAsync(CreatePropertyViewModel model, string agentId)
    {
        var code = await _propertyCodeDomainService.GenerateUniqueCodeAsync();
        var improvements = (await _improvementRepository.GetByIdsAsync(model.ImprovementIds)).ToList();
        var imageUrls = await _fileStorageService.SavePropertyImagesAsync(model.Images);

        var property = _propertyFactory.Create(
            agentId, model.PropertyTypeId, model.SaleTypeId, model.Price,
            model.Description, model.Size, model.Rooms, model.Bathrooms,
            imageUrls, improvements, code);

        await _propertyRepository.AddAsync(property);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PropertyViewModel>(property);
    }

    public async Task UpdateAsync(int id, EditPropertyViewModel model, string agentId)
    {
        var property = await _propertyRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Property), id);

        if (property.AgentId != agentId)
            throw new ForbiddenAccessException("No tiene permisos para modificar esta propiedad.");

        Domain.Rules.BusinessRuleValidator.CheckRule(
            new Domain.Rules.Property.PropertyMustBeAvailableToEditRule(property.Status));

        var improvements = (await _improvementRepository.GetByIdsAsync(model.ImprovementIds)).ToList();

        List<string>? newImageUrls = null;
        if (model.NewImages is { Count: > 0 })
            newImageUrls = await _fileStorageService.SavePropertyImagesAsync(model.NewImages);

        property.PropertyTypeId = model.PropertyTypeId;
        property.SaleTypeId = model.SaleTypeId;
        property.Price = model.Price;
        property.Description = model.Description;
        property.Size = model.Size;
        property.Rooms = model.Rooms;
        property.Bathrooms = model.Bathrooms;
        property.Improvements = improvements;

        if (newImageUrls is not null)
            property.Images = newImageUrls.Select(url => new Domain.Entities.PropertyImage { Url = url }).ToList();

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PropertyDto>> GetAllForApiAsync()
    {
        var properties = await _propertyRepository.GetAllAsync();
        var dtos = _mapper.Map<List<PropertyDto>>(properties);

        await PopulateAgentNamesAsync(dtos);

        return dtos;
    }

    public async Task<PropertyDto?> GetByIdForApiAsync(int id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property is null) return null;

        var dto = _mapper.Map<PropertyDto>(property);
        await PopulateAgentNameAsync(dto);

        return dto;
    }

    public async Task<PropertyDto?> GetByCodeForApiAsync(string code)
    {
        var property = await _propertyRepository.GetByCodeAsync(code);
        if (property is null) return null;

        var dto = _mapper.Map<PropertyDto>(property);
        await PopulateAgentNameAsync(dto);

        return dto;
    }

    public async Task DeleteAsync(int id, string agentId)
        => await _propertyDomainService.DeletePropertyWithValidationAsync(id, agentId);

    // ===== Helpers privados para completar AgentName (Domain no conoce Identity) =====

    private async Task PopulateAgentInfoAsync(PropertyViewModel vm)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(vm.AgentId, "Agent");
        if (agent is not null)
        {
            vm.AgentName = $"{agent.FirstName} {agent.LastName}";
            vm.AgentEmail = agent.Email;
            vm.AgentPhone = agent.PhoneNumber;
            vm.AgentPhotoUrl = agent.PhotoUrl;
        }
    }

    private async Task PopulateAgentInfoBatchAsync(List<PropertyViewModel> vms)
    {
        var agentCache = new Dictionary<string, UserSummary?>();
        foreach (var vm in vms)
        {
            if (!agentCache.TryGetValue(vm.AgentId, out var agent))
            {
                agent = await _authService.GetUserByIdInRoleAsync(vm.AgentId, "Agent");
                agentCache[vm.AgentId] = agent;
            }
            if (agent is not null)
            {
                vm.AgentName = $"{agent.FirstName} {agent.LastName}";
                vm.AgentEmail = agent.Email;
                vm.AgentPhone = agent.PhoneNumber;
                vm.AgentPhotoUrl = agent.PhotoUrl;
            }
        }
    }

    private async Task PopulateAgentNameAsync(PropertyDto dto)
    {
        var agent = await _authService.GetUserByIdInRoleAsync(dto.AgentId, "Agent");
        dto.AgentName = agent is not null ? $"{agent.FirstName} {agent.LastName}" : string.Empty;
    }

    private async Task PopulateAgentNamesAsync(List<PropertyDto> dtos)
    {
        var agentCache = new Dictionary<string, string>();

        foreach (var dto in dtos)
        {
            if (!agentCache.TryGetValue(dto.AgentId, out var agentName))
            {
                var agent = await _authService.GetUserByIdInRoleAsync(dto.AgentId, "Agent");
                agentName = agent is not null ? $"{agent.FirstName} {agent.LastName}" : string.Empty;
                agentCache[dto.AgentId] = agentName;
            }

            dto.AgentName = agentName;
        }
    }
}