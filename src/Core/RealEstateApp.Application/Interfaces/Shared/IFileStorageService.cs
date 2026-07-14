// Application/Interfaces/Shared/IFileStorageService.cs
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.Interfaces.Shared;

public interface IFileStorageService
{
    Task<List<string>> SavePropertyImagesAsync(List<IFormFile> images);
}