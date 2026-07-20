using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Infrastructure.Shared.Settings;

namespace RealEstateApp.Infrastructure.Shared.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _settings;

    public FileStorageService(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName)
    {
        // Crear la carpeta si no existe
        var folderPath = Path.Combine(_settings.BaseStoragePath, containerName);
        Directory.CreateDirectory(folderPath);

        // Generar nombre único para evitar colisiones
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(folderPath, uniqueFileName);

        using var output = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(output);

        // Retornar la ruta relativa para guardar en base de datos
        return Path.Combine(containerName, uniqueFileName);
    }

    public Task DeleteFileAsync(string fileUrl, string containerName)
    {
        var filePath = Path.Combine(_settings.BaseStoragePath, fileUrl);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
    public async Task<List<string>> SavePropertyImagesAsync(List<IFormFile> images)
    {
        var result = new List<string>();
        foreach (var image in images)
        {
            if (image.Length > 0)
            {
                using var stream = image.OpenReadStream();
                var url = await UploadFileAsync(stream, image.FileName, "Properties");
                result.Add(url);
            }
        }
        return result;
    }
}
