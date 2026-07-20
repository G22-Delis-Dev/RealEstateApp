using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IImprovementService : IGenericService<ImprovementViewModel>
{
    Task<ImprovementViewModel> CreateAsync(ImprovementViewModel model);
    Task UpdateAsync(int id, ImprovementViewModel model);

    Task<ImprovementDto> CreateForApiAsync(ImprovementRequestDto request);
    Task UpdateForApiAsync(int id, ImprovementRequestDto request);
}