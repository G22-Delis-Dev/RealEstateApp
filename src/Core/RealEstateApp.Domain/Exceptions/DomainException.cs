using System;

namespace RealEstateApp.Domain.Exceptions;

// Excepción base para reglas del dominio
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
