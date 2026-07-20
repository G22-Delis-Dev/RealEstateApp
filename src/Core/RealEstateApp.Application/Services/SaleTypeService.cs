using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class SaleTypeService : GenericService<SaleTypeViewModel, Domain.Entities.SaleType>, ISaleTypeService
{
    private readonly ISaleTypeRepository _saleTypeRepository;
    private readonly ISaleTypeFactory _saleTypeFactory;

    public SaleTypeService(
        ISaleTypeRepository saleTypeRepository,
        ISaleTypeFactory saleTypeFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(saleTypeRepository, unitOfWork, mapper)
    {
        _saleTypeRepository = saleTypeRepository;
        _saleTypeFactory = saleTypeFactory;
    }

    public async Task<SaleTypeViewModel> CreateAsync(SaleTypeViewModel model)
    {
        var exists = await _saleTypeRepository.NameExistsAsync(model.Name);
        var saleType = _saleTypeFactory.Create(model.Name, model.Description, exists);

        await _saleTypeRepository.AddAsync(saleType);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SaleTypeViewModel>(saleType);
    }

    public async Task UpdateAsync(int id, SaleTypeViewModel model)
    {
        var saleType = await _saleTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.SaleType), id);

        var exists = await _saleTypeRepository.NameExistsAsync(model.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(model.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "tipo de venta"));

        saleType.Name = model.Name.Trim();
        saleType.Description = model.Description;

        _saleTypeRepository.Update(saleType);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<SaleTypeDto> CreateForApiAsync(SaleTypeRequestDto request)
    {
        var exists = await _saleTypeRepository.NameExistsAsync(request.Name);
        var saleType = _saleTypeFactory.Create(request.Name, request.Description, exists);

        await _saleTypeRepository.AddAsync(saleType);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SaleTypeDto>(saleType);
    }

    public async Task UpdateForApiAsync(int id, SaleTypeRequestDto request)
    {
        var saleType = await _saleTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.SaleType), id);

        var exists = await _saleTypeRepository.NameExistsAsync(request.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(request.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "tipo de venta"));

        saleType.Name = request.Name.Trim();
        saleType.Description = request.Description;

        _saleTypeRepository.Update(saleType);
        await _unitOfWork.SaveChangesAsync();
    }
}