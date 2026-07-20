using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Seed;

public static class CatalogSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!context.PropertyTypes.Any())
        {
            context.PropertyTypes.AddRange(
                new PropertyType { Name = "Casa", Description = "Vivienda unifamiliar independiente." },
                new PropertyType { Name = "Apartamento", Description = "Unidad habitacional dentro de un edificio." },
                new PropertyType { Name = "Villa", Description = "Vivienda de lujo, usualmente con áreas exteriores amplias." },
                new PropertyType { Name = "Solar", Description = "Terreno disponible para construcción." },
                new PropertyType { Name = "Local comercial", Description = "Espacio destinado a uso comercial." }
            );
        }

        if (!context.SaleTypes.Any())
        {
            context.SaleTypes.AddRange(
                new SaleType { Name = "Venta", Description = "Transferencia definitiva de la propiedad." },
                new SaleType { Name = "Alquiler", Description = "Uso temporal de la propiedad mediante pago periódico." },
                new SaleType { Name = "Alquiler con opción a compra", Description = "Alquiler que permite adquirir la propiedad más adelante." }
            );
        }

        if (!context.Improvements.Any())
        {
            context.Improvements.AddRange(
                new Improvement { Name = "Piscina", Description = "Piscina privada dentro de la propiedad." },
                new Improvement { Name = "Marquesina", Description = "Espacio techado para vehículos." },
                new Improvement { Name = "Terraza", Description = "Área exterior techada o descubierta." },
                new Improvement { Name = "Seguridad 24 horas", Description = "Vigilancia permanente en la zona." },
                new Improvement { Name = "Ascensor", Description = "Elevador disponible en el edificio." },
                new Improvement { Name = "Planta eléctrica", Description = "Respaldo eléctrico ante apagones." },
                new Improvement { Name = "Área de lavado", Description = "Espacio dedicado para lavandería." }
            );
        }

        await context.SaveChangesAsync();
    }
}