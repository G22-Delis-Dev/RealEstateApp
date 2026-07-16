using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IImprovementService : IGenericService<ImprovementViewModel>
{
    Task<ImprovementViewModel> CreateAsync(ImprovementViewModel model);
    Task UpdateAsync(int id, ImprovementViewModel model);
}