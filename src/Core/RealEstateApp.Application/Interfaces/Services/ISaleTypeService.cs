using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface ISaleTypeService : IGenericService<SaleTypeViewModel>
{
    Task<SaleTypeViewModel> CreateAsync(SaleTypeViewModel model);
    Task UpdateAsync(int id, SaleTypeViewModel model);
}