using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IPropertyTypeService : IGenericService<PropertyTypeViewModel>
{
    Task<PropertyTypeViewModel> CreateAsync(PropertyTypeViewModel model);
    Task UpdateAsync(int id, PropertyTypeViewModel model);
    Task<PropertyTypeDto> CreateForApiAsync(PropertyTypeRequestDto request);
    Task UpdateForApiAsync(int id, PropertyTypeRequestDto request);
}