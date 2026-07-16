namespace RealEstateApp.Application.Interfaces.Shared;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName);
    Task DeleteFileAsync(string fileUrl, string containerName);
}
