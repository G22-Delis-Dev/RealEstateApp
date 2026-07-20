using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface ISaleTypeService : IGenericService<SaleTypeViewModel>
{
    Task<SaleTypeViewModel> CreateAsync(SaleTypeViewModel model);
    Task UpdateAsync(int id, SaleTypeViewModel model);

    Task<SaleTypeDto> CreateForApiAsync(SaleTypeRequestDto request);
    Task UpdateForApiAsync(int id, SaleTypeRequestDto request);
}