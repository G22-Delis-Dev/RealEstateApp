namespace RealEstateApp.Infrastructure.Shared.Settings;

public class FileStorageSettings
{
    // Ruta principal donde se guardarán los archivos localmente, o URL de un CDN/Cloud
    public string BaseStoragePath { get; set; } = null!;
}
