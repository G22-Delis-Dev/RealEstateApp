namespace RealEstateApp.Application.Interfaces.Services;

public interface IGenericService<TViewModel> where TViewModel : class
{
    Task<IEnumerable<TViewModel>> GetAllAsync();
    Task<TViewModel?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}