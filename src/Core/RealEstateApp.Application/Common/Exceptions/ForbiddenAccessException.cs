using System;

namespace RealEstateApp.Application.Common.Exceptions;

// Excepción para cuando el usuario no tiene los permisos requeridos
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Acceso denegado a este recurso.")
    {
    }

    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
