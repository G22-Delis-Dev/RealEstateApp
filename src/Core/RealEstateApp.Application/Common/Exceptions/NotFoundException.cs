using System;

namespace RealEstateApp.Application.Common.Exceptions;

// Excepción para cuando no se encuentra un recurso solicitado
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"El recurso '{name}' con clave '{key}' no fue encontrado.")
    {
    }
}
