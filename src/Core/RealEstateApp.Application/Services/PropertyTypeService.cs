// Services/PropertyTypeService.cs
using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class PropertyTypeService : GenericService<PropertyTypeViewModel, Domain.Entities.PropertyType>, IPropertyTypeService
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private readonly IPropertyTypeFactory _propertyTypeFactory;

    public PropertyTypeService(
        IPropertyTypeRepository propertyTypeRepository,
        IPropertyTypeFactory propertyTypeFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(propertyTypeRepository, unitOfWork, mapper)
    {
        _propertyTypeRepository = propertyTypeRepository;
        _propertyTypeFactory = propertyTypeFactory;
    }

    public async Task<PropertyTypeViewModel> CreateAsync(PropertyTypeViewModel model)
    {
        var exists = await _propertyTypeRepository.NameExistsAsync(model.Name);
        var propertyType = _propertyTypeFactory.Create(model.Name, model.Description, exists);

        await _propertyTypeRepository.AddAsync(propertyType);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PropertyTypeViewModel>(propertyType);
    }

    public async Task UpdateAsync(int id, PropertyTypeViewModel model)
    {
        var propertyType = await _propertyTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.PropertyType), id);

        var exists = await _propertyTypeRepository.NameExistsAsync(model.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(model.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "tipo de propiedad"));

        propertyType.Name = model.Name.Trim();
        propertyType.Description = model.Description;

        _propertyTypeRepository.Update(propertyType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PropertyTypeDto> CreateForApiAsync(PropertyTypeRequestDto request)
    {
        var exists = await _propertyTypeRepository.NameExistsAsync(request.Name);
        var propertyType = _propertyTypeFactory.Create(request.Name, request.Description, exists);

        await _propertyTypeRepository.AddAsync(propertyType);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PropertyTypeDto>(propertyType);
    }

    public async Task UpdateForApiAsync(int id, PropertyTypeRequestDto request)
    {
        var propertyType = await _propertyTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.PropertyType), id);

        var exists = await _propertyTypeRepository.NameExistsAsync(request.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(request.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "tipo de propiedad"));

        propertyType.Name = request.Name.Trim();
        propertyType.Description = request.Description;

        _propertyTypeRepository.Update(propertyType);
        await _unitOfWork.SaveChangesAsync();
    }
}