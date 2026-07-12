using System;

namespace RealEstateApp.Application.Common.Exceptions;

// Excepción para cuando existe un conflicto de estado, por ejemplo duplicidad de datos
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
