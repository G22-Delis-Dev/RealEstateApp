// Services/ImprovementService.cs
using AutoMapper;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class ImprovementService : GenericService<ImprovementViewModel, Domain.Entities.Improvement>, IImprovementService
{
    private readonly IImprovementRepository _improvementRepository;
    private readonly IImprovementFactory _improvementFactory;

    public ImprovementService(
        IImprovementRepository improvementRepository,
        IImprovementFactory improvementFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(improvementRepository, unitOfWork, mapper)
    {
        _improvementRepository = improvementRepository;
        _improvementFactory = improvementFactory;
    }

    public async Task<ImprovementViewModel> CreateAsync(ImprovementViewModel model)
    {
        var exists = await _improvementRepository.NameExistsAsync(model.Name);
        var improvement = _improvementFactory.Create(model.Name, model.Description, exists);

        await _improvementRepository.AddAsync(improvement);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ImprovementViewModel>(improvement);
    }

    public async Task UpdateAsync(int id, ImprovementViewModel model)
    {
        var improvement = await _improvementRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Improvement), id);

        var exists = await _improvementRepository.NameExistsAsync(model.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(model.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "mejora"));

        improvement.Name = model.Name.Trim();
        improvement.Description = model.Description;

        _improvementRepository.Update(improvement);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ImprovementDto> CreateForApiAsync(ImprovementRequestDto request)
    {
        var exists = await _improvementRepository.NameExistsAsync(request.Name);
        var improvement = _improvementFactory.Create(request.Name, request.Description, exists);

        await _improvementRepository.AddAsync(improvement);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ImprovementDto>(improvement);
    }

    public async Task UpdateForApiAsync(int id, ImprovementRequestDto request)
    {
        var improvement = await _improvementRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Improvement), id);

        var exists = await _improvementRepository.NameExistsAsync(request.Name, excludeId: id);

        Domain.Rules.BusinessRuleValidator.CheckRules(
            new Domain.Rules.Catalog.NameMustNotBeEmptyOrWhitespaceRule(request.Name),
            new Domain.Rules.Catalog.NameMustBeUniqueRule(exists, "mejora"));

        improvement.Name = request.Name.Trim();
        improvement.Description = request.Description;

        _improvementRepository.Update(improvement);
        await _unitOfWork.SaveChangesAsync();
    }
}