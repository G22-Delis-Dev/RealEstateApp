using RealEstateApp.Application.ViewModels.Catalogs;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IPropertyTypeService : IGenericService<PropertyTypeViewModel>
{
    Task<PropertyTypeViewModel> CreateAsync(PropertyTypeViewModel model);
    Task UpdateAsync(int id, PropertyTypeViewModel model);
}