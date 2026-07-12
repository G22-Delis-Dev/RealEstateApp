using System;

namespace RealEstateApp.Domain.Exceptions;

// Excepción para cuando una regla de negocio del dominio no se cumple
public class BusinessRuleValidationException : DomainException
{
    public string Details { get; }

    public BusinessRuleValidationException(string message) : base(message)
    {
        Details = string.Empty;
    }

    public BusinessRuleValidationException(string message, string details) : base(message)
    {
        Details = details;
    }
}
