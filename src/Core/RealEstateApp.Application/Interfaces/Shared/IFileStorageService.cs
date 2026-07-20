using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.Interfaces.Shared;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName);
    Task DeleteFileAsync(string fileUrl, string containerName);
    Task<List<string>> SavePropertyImagesAsync(List<IFormFile> images);
}
